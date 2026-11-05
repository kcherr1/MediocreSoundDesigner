using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Lame;

namespace MediocreSoundDesigner;

public enum SampleRegion {
    BEGINNING_ONLY,
    END_ONLY,
    BEGINNING_AND_END,
    ANYWHERE,
}

public class Track : IDisposable
{
    // Every track stored in this format, no matter where it came from
    public const int INTERNAL_SAMPLE_RATE = 44100;
    public const int INTERNAL_CHANNELS = 1;
    private const float WAVEFORM_NOISE_FLOOR = 0.005f;
    private const float MAX_WAVEFORM_DISPLAY_SCALE = 20.0f;

    // Fields
    private string file;
    private TimeSpan duration;
    private float offset;
    private float timeScale;
    private int sampleRate;
    private float[] samples;
    private float waveformDisplayScale;
    private float pitch;
    private AudioFileReader fileReader; // playback only
    private WaveOut outputDevice;
    private Panel panContainer;
    private int drawTimescale;

    // properties
    public int DrawOffset { get; set; }
    public bool IsDisabled { get; private set; }
    public Panel PanDrawSamples { get; private set; }
    public float Volume { get; private set; }
    private string tempFilePath => Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", "temp.wav");
    public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;
    public int? SelectionStartSample { get; set; } = null;
    public int? SelectionEndSample { get; set; } = null;
    public bool HasSelection => SelectionStartSample.HasValue && SelectionEndSample.HasValue;
    public int SampleCount => samples?.Length ?? 0;

    private static WaveFormat OutputFormat => WaveFormat.CreateIeeeFloatWaveFormat(INTERNAL_SAMPLE_RATE, INTERNAL_CHANNELS);
    public TimeSpan CurrentTime => fileReader?.CurrentTime ?? TimeSpan.Zero;
    public TimeSpan TotalTime => fileReader?.TotalTime ?? TimeSpan.Zero;
    public String GetFileName() => Path.GetFileName(file);
    
    // ctor
    public Track(string file)
    {
        using AudioFileReader reader = new(file);
        ISampleProvider provider = reader;

        if (reader.WaveFormat.Channels == 2)
        {
            provider = new StereoToMonoSampleProvider(provider) { LeftVolume = 0.5f, RightVolume = 0.5f };
        }
        else if (reader.WaveFormat.Channels > 2)
        {
            throw new NotSupportedException("Only mono and stereo files are supported.");
        }

        if (provider.WaveFormat.SampleRate != INTERNAL_SAMPLE_RATE)
        {
            provider = new WdlResamplingSampleProvider(provider, INTERNAL_SAMPLE_RATE);
        }

        List<float> loaded = new();
        float[] buffer = new float[INTERNAL_SAMPLE_RATE];
        int read;

        // translate from mono -> stereo and vice versa
        while ((read = provider.Read(buffer)) > 0)
        {
            loaded.AddRange(new ArraySegment<float>(buffer, 0, read));
        }

        this.samples = loaded.ToArray();
        waveformDisplayScale = CalculateWaveformDisplayScale();
        this.file = file;
        this.duration = TimeSpan.FromSeconds((double)samples.Length / INTERNAL_SAMPLE_RATE);
        this.offset = 0;
        this.timeScale = 1.0f;
        this.Volume = 1.0f;
        this.pitch = 1.0f;
        this.sampleRate = INTERNAL_SAMPLE_RATE;
        IsDisabled = false;
    }
    public Track(float[] samples)
    {
        this.samples = samples;
        waveformDisplayScale = CalculateWaveformDisplayScale();

        this.file = "";
        this.duration = TimeSpan.Zero;
        this.offset = 0;
        this.timeScale = 1.0f;
        this.Volume = 1.0f;
        this.pitch = 1.0f;
        this.sampleRate = INTERNAL_SAMPLE_RATE;
        IsDisabled = false;
    }
    public void LinkPanels(Panel panDrawSamples, Panel panContainer) {
        this.PanDrawSamples = panDrawSamples;
        this.panContainer = panContainer;
    }

    // edits
    public (int, int) GetTargetRange()
    {
        if (HasSelection)
        {
            return (SelectionStartSample.Value, SelectionEndSample.Value);
        }
        return (0, samples.Length);
    }
    public void ClearSelection()
    {
        SelectionStartSample = null;
        SelectionEndSample = null;
    }
    public void SetSelection(int startSample, int endSample)
    {
        int start = Math.Clamp(Math.Min(startSample, endSample), 0, samples.Length);
        int end = Math.Clamp(Math.Max(startSample, endSample), 0, samples.Length);
        if (start == end)
        {
            ClearSelection();
        }
        else
        {
            SelectionStartSample = start;
            SelectionEndSample = end;
        }
    }
    public float[] CloneSamples() {
    return (float[])samples.Clone();
    }
    public void ReplaceSamples(float[] samples) {
    this.samples = samples;
    waveformDisplayScale = CalculateWaveformDisplayScale();
    }
    public void Trim() {
    }
    public void AlterTimeScale(float scale) {
    }
    public void SetVolume(float newVolume) {
    Volume = newVolume;
    }
    public void Flip()
    {
        var (start, end) = GetTargetRange();
        for (int i = start; i < end; i++)
        {
            samples[i] *= -1;
        }
    }

    public void Reverse()
    {
        var (start, end) = GetTargetRange();
        int len = end - start;
        for (int i = 0; i < len / 2; i++)
        {
            var temp = samples[start + i];
            samples[start + i] = samples[end - 1 - i];
            samples[end - 1 - i] = temp;
        }
    }

    public void ApplyVolume(float factor)
    {
        var (start, end) = GetTargetRange();
        for (int i = start; i < end; i++)
        {
            samples[i] *= factor;
        }
    }

    public void EliminateSilence(SampleRegion region, float minDuration, float thres)
    {
        var (start, end) = GetTargetRange();
        float[] targetSlice = samples[start..end];

        if (region is SampleRegion.BEGINNING_ONLY or SampleRegion.BEGINNING_AND_END)
        {
            int silentDurationInNumSamples = 0;
            for (int i = 0; i < targetSlice.Length; i++)
            {
                if (Math.Abs(targetSlice[i]) <= thres)
                {
                    silentDurationInNumSamples++;
                }
                else
                {
                    break;
                }
            }
            if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
            {
                targetSlice = targetSlice[silentDurationInNumSamples..];
            }
        }

        if (region is SampleRegion.END_ONLY or SampleRegion.BEGINNING_AND_END)
        {
            int silentDurationInNumSamples = 0;
            for (int i = targetSlice.Length - 1; i >= 0; i--)
            {
                if (Math.Abs(targetSlice[i]) <= thres)
                {
                    silentDurationInNumSamples++;
                }
                else
                {
                    break;
                }
            }
            if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
            {
                targetSlice = targetSlice[0..^silentDurationInNumSamples];
            }
        }

        if (region is SampleRegion.ANYWHERE)
        {
            int silentDurationInNumSamples = 0;
            int silentRegionStart = 0;
            int i;
            for (i = 0; i < targetSlice.Length; i++)
            {
                if (Math.Abs(targetSlice[i]) <= thres)
                {
                    silentDurationInNumSamples++;
                }
                else
                {
                    if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
                    {
                        targetSlice = [.. targetSlice[0..silentRegionStart], .. targetSlice[i..]];
                        i = 0;
                    }
                    silentRegionStart = i + 1;
                    silentDurationInNumSamples = 0;
                }
            }
            if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
            {
                targetSlice = [.. targetSlice[0..silentRegionStart], .. targetSlice[i..]];
            }
        }

        // Splice altered slice back into track buffer
        samples = [.. samples[0..start], .. targetSlice, .. samples[end..]];
        waveformDisplayScale = CalculateWaveformDisplayScale();
        ClearSelection();
    }

    private float CalculateWaveformDisplayScale()
    {
        float peak = 0.0f;
        foreach (float sample in samples)
        {
            peak = Math.Max(peak, Math.Abs(sample));
        }

        return peak > 0.0f
            ? Math.Min(1.0f / peak, MAX_WAVEFORM_DISPLAY_SCALE)
            : 1.0f;
    }

    // play
    public void Play() {
        outputDevice?.Dispose();
        fileReader?.Dispose();
        SaveToTempFile();
        fileReader = new AudioFileReader(tempFilePath);
        outputDevice = new WaveOut();
        outputDevice.Init(fileReader);
        outputDevice.Play();
    }
    public void Pause() {
    outputDevice?.Pause();
    }
    public void Reset() {
    }
    public int GetPlayheadPos() {
    return (int)((outputDevice?.GetPosition() ?? 0) / 4);
    }
    public void ToggleDisabled() {
    IsDisabled = !IsDisabled;
    }

    // file i/o

    private void SaveToTempFile()
    {
        SaveToFile(tempFilePath);
    }
    public void SaveToFile(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        float[] adjSamples = GetAdjustedSamples();

        if (Path.GetExtension(path).Equals(".mp3", StringComparison.OrdinalIgnoreCase))
        {
            WaveFormat pcmFormat = new(INTERNAL_SAMPLE_RATE, 16, INTERNAL_CHANNELS);
            using var writer = new LameMP3FileWriter(path, pcmFormat, 192);
            byte[] buffer = new byte[adjSamples.Length * 2];
            for (int i = 0; i < adjSamples.Length; i++)
            {
                short sample = (short)Math.Clamp(adjSamples[i] * short.MaxValue, short.MinValue, short.MaxValue);
                buffer[i * 2] = (byte)(sample & 0xff);
                buffer[i * 2 + 1] = (byte)(sample >> 8);
            }
            writer.Write(buffer, 0, buffer.Length);
        }
        else
        {
            using var writer = new WaveFileWriter(path, OutputFormat);
            writer.WriteSamples(adjSamples, 0, adjSamples.Length);
        }
    }

    public float[] GetAdjustedSamples() {
    float[] adjSamples = new float[samples.Length];
    for (int s = 0; s < samples.Length; s++) {
        adjSamples[s] = samples[s] * Volume;
    }
    float[] deadSpace = new float[DrawOffset * drawTimescale];
    for (int i = 0; i < DrawOffset; i++) {
        deadSpace[i] = 0.0f;
    }

    return [.. deadSpace, .. adjSamples];
    }

    // render
    public void ShiftSampleOver(int amount) {
    DrawOffset += amount;
    DrawOffset = Math.Max(0, DrawOffset);
    }
    public void DrawSamples(Graphics g, int drawTimescale, int? selStartX = null, int? selEndX = null)
    {
        if (PanDrawSamples is null)
        {
            return;
        }

        this.drawTimescale = drawTimescale;
        int height = PanDrawSamples.Height;

        // 1. Draw Waveform
        float heightAdj = height / 2f;
        if (samples != null && samples.Length > 0)
        {
            int x = DrawOffset;
            using Pen wavePen = new(IsDisabled ? Settings.Color.TrackSamples_Disabled : Settings.Color.TrackSamples_Enabled);
            List<Point> points = new();
            for (int s = 0; s < samples.Length; s += drawTimescale)
            {
                float audioSample = Math.Abs(samples[s]) < WAVEFORM_NOISE_FLOOR ? 0.0f : samples[s];
                var sample = (int)(audioSample * waveformDisplayScale * heightAdj * Volume);
                points.Add(new(x, height / 2 - sample));
                x++;
            }
            if (points.Count > 1)
            {
                g.DrawLines(wavePen, points.ToArray());
            }
        }

        // 2. Draw Unified Selection Box across the track
        if (selStartX.HasValue && selEndX.HasValue && selStartX.Value != selEndX.Value)
        {
            int startX = selStartX.Value;
            int endX = selEndX.Value;
            int width = Math.Max(1, endX - startX);

            using (SolidBrush fillBrush = new(Settings.Color.TrackSelection))
            {
                g.FillRectangle(fillBrush, startX, 0, width, height);
            }

            using (Pen borderPen = new(Color.White, 1.5f))
            {
                g.DrawLine(borderPen, startX, 0, endX, 0);
                g.DrawLine(borderPen, startX, height - 1, endX, height - 1);
                g.DrawLine(borderPen, startX, 0, startX, height);
                g.DrawLine(borderPen, endX, 0, endX, height);
            }
        }
    }

    // dispose
    public void Dispose() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
    }
}

using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MediocreSoundDesigner;

public enum SampleRegion
{
    BEGINNING_ONLY,
    END_ONLY,
    BEGINNING_AND_END,
    ANYWHERE,
}

public class Track : IDisposable
{
    // Fields
    private string file;
    private TimeSpan duration;
    //private float offset;
    //private float timeScale;
    private int sampleRate;
    private float[] samples;
    //private float pitch;
    private int bits;
    private int channels;
    private AudioFileReader fileReader;
    private WaveOut outputDevice;
    private Panel panContainer;
    private int drawTimescale;
    private WaveFormat overrideFormat;

    private float[] samplesBeforeEffect;

   // properties
    public int DrawOffset { get; set; }
    public bool IsDisabled { get; private set; }
    public Panel PanDrawSamples { get; private set; }
    public float Volume { get; private set; }
    public System.Drawing.Color TrackColor { get; private set; } = System.Drawing.Color.Empty;
    public string DisplayName { get; private set; }
    private readonly string tempFilePath = Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", $"{Guid.NewGuid():N}.wav");
    public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;
    public WaveFormat Format => fileReader?.WaveFormat ?? overrideFormat ?? new WaveFormat(); 
    public int SampleValuesPerSecond {
        get {
            WaveFormat format = fileReader?.WaveFormat ?? overrideFormat ?? new WaveFormat();
            int rate = sampleRate > 0 ? sampleRate : format.SampleRate;
            int channelCount = channels > 0 ? channels : format.Channels;
            return rate * channelCount;
        }
    }

    // ctor
    public Track(string file)
    {
        fileReader = new AudioFileReader(file);
        int sampleCount = (int)(fileReader.Length / 4);
        this.samples = new float[sampleCount];
        int samplesRead = ((ISampleProvider)fileReader).Read(samples);
        if (samplesRead < sampleCount)
        {
            Array.Resize(ref samples, samplesRead);
        }

  this.file = file;
  DisplayName = Path.GetFileNameWithoutExtension(file);
  this.duration = fileReader.TotalTime;
  //this.offset = 0;
  //this.timeScale = 1.0f;
  Volume = 1.0f;
  //this.pitch = 1.0f;
  this.bits = fileReader.WaveFormat.BitsPerSample;
  this.channels = fileReader.WaveFormat.Channels;
  this.sampleRate = fileReader.WaveFormat.SampleRate;
  IsDisabled = false;
  }

  public Track(float[] samples) {
    this.samples = samples;
    this.file = "";
    DisplayName = "Generated Track";
    this.duration = TimeSpan.Zero;
   // this.offset = 0;
   // this.timeScale = 1.0f;
    Volume = 1.0f;
   // this.pitch = 1.0f;
    this.bits = 32;
    this.channels = 1;
    this.sampleRate = 0;
    IsDisabled = false;
  }
  public Track(TrackData data) : this((float[])data.Samples.Clone()) {
    sampleRate = data.SampleRate;
    channels = data.Channels;
    Volume = data.Volume;
    DrawOffset = data.DrawOffset;
    IsDisabled = data.IsDisabled;
    DisplayName = string.IsNullOrWhiteSpace(data.DisplayName) ? "Generated Track" : data.DisplayName;
    TrackColor = data.TrackColorArgb.HasValue
      ? System.Drawing.Color.FromArgb(data.TrackColorArgb.Value)
      : System.Drawing.Color.Empty;

    if (sampleRate > 0 && channels > 0) {
        duration = TimeSpan.FromSeconds((double)samples.Length / sampleRate / channels);
    }
  }

  public Track(float[] samples, WaveFormat format) : this(samples) {
      overrideFormat = format;
      sampleRate = format.SampleRate;
      channels = format.Channels;
      bits = format.BitsPerSample;

      if (sampleRate > 0 && channels > 0) {
          duration = TimeSpan.FromSeconds((double)samples.Length / sampleRate / channels);
      }
  }

  public void LinkPanels(Panel panDrawSamples, Panel panContainer) {
      this.PanDrawSamples = panDrawSamples;
      this.panContainer = panContainer;
  }

  public void SetDisplayName(string name)
  {
    DisplayName = name;
    PanDrawSamples?.Invalidate();
  }

  // edits
  public float[] CloneSamples() {
    return (float[])samples.Clone();
  }

  public void ReplaceSamples(float[] samples) {
    this.samples = samples;
  }

  public void Reverse() {
    for (int i = 0; i < samples.Length / 2; i++) {
      var temp = samples[i];
      samples[i] = samples[samples.Length - 1 - i];
      samples[samples.Length - 1 - i] = temp;
    }
  }

  public void Flip() {
    for (int i = 0; i < samples.Length; i++) {
      samples[i] *= -1;
    }
  }

  public void Trim() {
  }

  // Keep only the audio between two timeline pixel positions.
  public void Trim(int startPx, int endPx, int scale) {
    if (scale <= 0 || channels <= 0 || samples is null) return;

    int start = Math.Clamp((startPx - DrawOffset) * scale, 0, samples.Length);
    int end = Math.Clamp((endPx - DrawOffset) * scale, 0, samples.Length);
    start -= start % channels;
    end -= end % channels;
    if (end <= start) return;

    samples = samples[start..end];
    DrawOffset = Math.Max(DrawOffset, startPx);
  }

  // Remove the audio between two timeline pixel positions.
  public void DeletePart(int startPx, int endPx, int scale) {
    if (scale <= 0 || channels <= 0 || samples is null) return;

    int start = Math.Clamp((startPx - DrawOffset) * scale, 0, samples.Length);
    int end = Math.Clamp((endPx - DrawOffset) * scale, 0, samples.Length);
    start -= start % channels;
    end -= end % channels;
    if (end <= start) return;

    samples = [.. samples[..start], .. samples[end..]];
  }

  // Draw a translucent overlay over the current selection.
  public void ShowSelection(Graphics g, int startPx, int endPx) {
    if (PanDrawSamples is null || endPx <= startPx) return;
    using Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(35, 65, 55, 135));
    g.FillRectangle(brush, startPx, 0, endPx - startPx, PanDrawSamples.Height);
  }


	public void AlterTimeScale(float scale) {
  }

  public void SetVolume(float newVolume) {
    Volume = newVolume;
  }



	// effect = "Deep", "Chipmunk", "VoiceOnly" or "RemoveVoice"
	public void ApplyEffect(string effect)
	{
		int rate = sampleRate > 0 ? sampleRate : Format.SampleRate;
		int ch = channels > 0 ? channels : Format.Channels;

		samplesBeforeEffect = CloneSamples();      // save the old sound so Undo works
		float[] result = CloneSamples();           // work on a copy
                                                   //   int fadeLength = rate * 2;
        float fadeSeconds = 5;
        int fadeLength = (int)(rate * fadeSeconds);
		switch (effect)
		{
			case "Deep":
				result = ChangePitch(result, rate, ch, 0.75f);    // lower voice
				break;

			case "Chipmunk":
				result = ChangePitch(result, rate, ch, 1.5f);     // higher voice
				break;

			case "VoiceOnly":
				result = KeepVoiceOnly(result, rate, ch);         // quiet the music
				break;

            case "FadeIn":
                {
                    int totalFames = result.Length / ch;
                    fadeLength = Math.Min(fadeLength, totalFames);

                    for (int i = 0; i < result.Length; i++)
                    {
                        int frame = i / ch;
                        if (frame >= fadeLength) break;
                        result[i] *= (float)frame / fadeLength;

                    }
                    break;
                }

            case "FadeOut":
                {
                    int totalFrames = result.Length / ch;
                    fadeLength = Math.Min(fadeLength, totalFrames);

                    
                    for (int i = 0; i < result.Length; i++)
                    {
                        int framesLeft = totalFrames - i / ch;
                        if (framesLeft < fadeLength)
                            result[i] *= (float)framesLeft / fadeLength;
                    }
                    break;
                }
                
			case "RemoveVoice":
                if (ch != 2)                                      // needs left + right channels
                {
                    MessageBox.Show("vocice removal need a stero track (left + right).");
                    break;
                }
                float biggestDifference = 0;
                for (int i = 0; i + 1 < result.Length; i += 2)
                    biggestDifference = Math.Max(biggestDifference, Math.Abs(result[i] - result[i + 1]));
                if (biggestDifference < 0.01f)
                {
                    MessageBox.Show("left and right ");
                    break;
                }
                for (int i =0; i +1 < result.Length; i += 2)
                {
                    float difference = (result[i] - result[i + 1]) / 2;
                    result[i] = difference;
                    result[i + 1] = difference;
                }
                break;
				
		}

		samples = result;
	}

	// Changes the pitch with NAudio's built-in pitch shifter.
	// pitch 1.0 = same, below 1 = deeper, above 1 = higher. The length stays the same.
	private float[] ChangePitch(float[] input, int rate, int ch, float pitch)
	{
		// turn our float array into something NAudio can read
		byte[] bytes = new byte[input.Length * 4];
		Buffer.BlockCopy(input, 0, bytes, 0, bytes.Length);
		var raw = new RawSourceWaveStream(new MemoryStream(bytes), WaveFormat.CreateIeeeFloatWaveFormat(rate, ch));

		var shifter = new SmbPitchShiftingSampleProvider(raw.ToSampleProvider(), 4096, 4, pitch);

		// read everything back out into a new array
		float[] output = new float[input.Length];
		int total = 0;
		while (total < output.Length)
		{
			int read = shifter.Read(output.AsSpan(total));
			if (read == 0) break;
			total += read;
		}
		return output;
	}

	// Keeps the human voice and quiets the music (a rough method, not AI).
	private float[] KeepVoiceOnly(float[] input, int rate, int ch)
	{
		float[] output = new float[input.Length];

		// STEP 1: singers are usually mixed in the CENTER, so left and right both contain the voice.
		// Averaging the channels keeps the center and weakens sounds placed at the sides.
		for (int i = 0; i + ch - 1 < input.Length; i += ch)
		{
			float sum = 0;
			for (int c = 0; c < ch; c++)
				sum += input[i + c];

			float middle = sum / ch;
			for (int c = 0; c < ch; c++)
				output[i + c] = middle;
		}

		// STEP 2: speech mostly lives between 200 Hz and 4000 Hz.
		// Cut the deep bass below it and the sharp cymbals above it.
		var highPass = new NAudio.Dsp.BiQuadFilter[ch];
		var lowPass = new NAudio.Dsp.BiQuadFilter[ch];
		for (int c = 0; c < ch; c++)
		{
			highPass[c] = NAudio.Dsp.BiQuadFilter.HighPassFilter(rate, 200, 1);
			lowPass[c] = NAudio.Dsp.BiQuadFilter.LowPassFilter(rate, 4000, 1);
		}
		for (int i = 0; i < output.Length; i++)
			output[i] = lowPass[i % ch].Transform(highPass[i % ch].Transform(output[i]));

		// STEP 3: filtering made it quieter, so scale it back up (loudest sample = 0.9)
		float peak = 0;
		foreach (float s in output)
			peak = Math.Max(peak, Math.Abs(s));
		if (peak > 0)
			for (int i = 0; i < output.Length; i++)
				output[i] = output[i] / peak * 0.9f;

		return output;
	}

	public bool CanUndoEffect => samplesBeforeEffect != null;

	public void UndoEffect()
	{
		if (samplesBeforeEffect == null) return;
		samples = samplesBeforeEffect;
		samplesBeforeEffect = null;
	}

	public void SetTrackColor(System.Drawing.Color color) {
    TrackColor = color;

    if (PanDrawSamples is not null) {
      PanDrawSamples.BackColor = color;
      PanDrawSamples.Invalidate();
    }
  } 
    
    public void EliminateSilence(SampleRegion region, float minDuration, float thres)
    {
        float[] newSamples = CloneSamples();
        if (region is SampleRegion.BEGINNING_ONLY or SampleRegion.BEGINNING_AND_END)
        {
            int silentDurationInNumSamples = 0;
            for (int i = 0; i < newSamples.Length; i++)
            {
                if (Math.Abs(newSamples[i]) <= thres)
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
                newSamples = newSamples[silentDurationInNumSamples..];
            }
        }
        if (region is SampleRegion.END_ONLY or SampleRegion.BEGINNING_AND_END)
        {
            int silentDurationInNumSamples = 0;
            for (int i = newSamples.Length - 1; i >= 0; i--)
            {
                if (Math.Abs(newSamples[i]) <= thres)
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
                newSamples = newSamples[0..^silentDurationInNumSamples];
            }
        }
        if (region is SampleRegion.ANYWHERE)
        {
            int silentDurationInNumSamples = 0;
            int silentRegionStart = 0;
            int i;
            for (i = 0; i < newSamples.Length; i++)
            {
                if (Math.Abs(newSamples[i]) <= thres)
                {
                    silentDurationInNumSamples++;
                }
                else
                {
                    if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
                    {
                        newSamples = [.. newSamples[0..silentRegionStart], .. newSamples[i..]];
                        i = 0;
                    }
                    silentRegionStart = i + 1;
                    silentDurationInNumSamples = 0;
                }
            }
            if ((float)silentDurationInNumSamples / sampleRate >= minDuration)
            {
                newSamples = [.. newSamples[0..silentRegionStart], .. newSamples[i..]];
            }
        }
        samples = newSamples;
    }

  // play
  public void Play() {
    if (State == PlaybackState.Paused) {
      outputDevice.Play();
      return;
    }

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

  public void Resume()
  {
    outputDevice?.Play();
  }
  public void Reset() {
    outputDevice?.Stop();
    if (fileReader is not null) fileReader.Position = 0;
  }
  public int GetPlayheadPos() {
    if (outputDevice is null || fileReader is null) return 0;

    int readerBytesPerValue = fileReader.WaveFormat.BitsPerSample / 8;
    if (readerBytesPerValue <= 0) return 0;

    long totalValues = fileReader.Length / readerBytesPerValue;
    if (State == PlaybackState.Stopped && fileReader.Position >= fileReader.Length) {
      return (int)totalValues;
    }

    int outputBytesPerValue = outputDevice.OutputWaveFormat.BitsPerSample / 8;
    if (outputBytesPerValue <= 0) return 0;

    long playedValues = outputDevice.GetPosition() / outputBytesPerValue;
    return (int)Math.Clamp(playedValues, 0, totalValues);
  }

  public double GetPlaybackProgress() {
    double totalSeconds = GetDurationSeconds();
    if (totalSeconds <= 0) return 0;
    return Math.Clamp(GetPlaybackSeconds() / totalSeconds, 0, 1);
  }

  public double GetPlaybackSeconds() {
    if (outputDevice is null || fileReader is null || SampleValuesPerSecond <= 0) return 0;
    double seconds = GetPlayheadPos() / (double)SampleValuesPerSecond;
    return Math.Min(seconds, GetDurationSeconds());
  }

  public double GetDurationSeconds() {
    return SampleValuesPerSecond > 0 ? samples.Length / (double)SampleValuesPerSecond : 0;
  }
  public void ToggleDisabled() {
    IsDisabled = !IsDisabled;
  }
    public TrackData GetProjectData() {
    return new TrackData {
        Samples = CloneSamples(),
        SampleRate = sampleRate,
        Channels = channels,
        Volume = Volume,
        DrawOffset = DrawOffset,
        IsDisabled = IsDisabled,
        DisplayName = DisplayName,
        TrackColorArgb = TrackColor.IsEmpty ? null : TrackColor.ToArgb(),
    };
  }

// file i/o
  private void SaveToTempFile() {
      Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
      using var writer = new WaveFileWriter(tempFilePath, fileReader?.WaveFormat ?? new());
      writer.WriteSamples(GetAdjustedSamples(), 0, samples.Length);
  }

  public void SaveToFile(string path) {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      if (Path.GetExtension(path).Equals(".mp3", StringComparison.OrdinalIgnoreCase)) {
          SaveToMp3(path);
      }
      else {
          SaveToWav(path);
      }
  }
    private void SaveToWav(string path)
    {
        float[] adj = GetAdjustedSamples();
        using var writer = new WaveFileWriter(path, Format);
        writer.WriteSamples(adj, 0, adj.Length);
    }
    private void SaveToMp3(string path)
    {
        string tempWav = Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", "export.wav");
        Directory.CreateDirectory(Path.GetDirectoryName(tempWav));
        try
        {
            SaveToWav(tempWav);
            MediaFoundationApi.Startup();
            using var reader = new AudioFileReader(tempWav);
            ISampleProvider sp = reader;
            if (reader.WaveFormat.SampleRate is not (32000 or 44100 or 48000))
            {
                sp = new WdlResamplingSampleProvider(sp, 44100);
            }
            MediaFoundationEncoder.EncodeToMp3(new SampleToWaveProvider16(sp), path, 192000);
        }
        catch
        {
            if (File.Exists(path)) File.Delete(path);
            throw;
        }
        finally
        {
            if (File.Exists(tempWav)) File.Delete(tempWav);
        }
    }
    public float[] GetAdjustedSamples()
    {
        float[] adjSamples = new float[samples.Length];
        for (int s = 0; s < samples.Length; s++)
        {
            adjSamples[s] = samples[s] * Volume;
        }
        float[] deadSpace = new float[DrawOffset * drawTimescale];
        for (int i = 0; i < DrawOffset; i++)
        {
            deadSpace[i] = 0.0f;
        }

        return [.. deadSpace, .. adjSamples];
    }

    // render
    public void ShiftSampleOver(int amount)
    {
        DrawOffset += amount;
        DrawOffset = Math.Max(0, DrawOffset);
    }
  public void DrawSamples(Graphics g, int drawTimescale, int horizontalScrollOffset = 0)
  {
    if (PanDrawSamples is null || samples is null || samples.Length == 0)
    {
      return;
    }

    int samplesPerPixel = Math.Max(1, drawTimescale);
    this.drawTimescale = samplesPerPixel;

    float headerHeight = 30f;
    float waveformHeight = Math.Max(1f, PanDrawSamples.ClientSize.Height - headerHeight);
    float centerY = headerHeight + waveformHeight / 2f;
    float amplitude = waveformHeight / 2f * Volume;

    Color waveformColor = IsDisabled
        ? Settings.Color.TrackSamples_Disabled
        : Settings.Color.TrackSamples_Enabled;

    RectangleF baselineBounds = g.VisibleClipBounds;

    using Pen baselinePen = new(Color.FromArgb(70, waveformColor));
    g.DrawLine(
        baselinePen,
        baselineBounds.Left,
        centerY,
        baselineBounds.Right,
        centerY);

    // Keep the waveform dark, but let some of the track color show through.
    using Pen waveformPen = new(Color.FromArgb(
        190,
        waveformColor.R,
        waveformColor.G,
        waveformColor.B));

    // At maximum zoom, connect individual samples to show their shape.
    if (samplesPerPixel == 1)
    {
      RectangleF sampleClip = g.VisibleClipBounds;

      int firstSample = Math.Clamp(
          (int)Math.Floor(sampleClip.Left - DrawOffset),
          0,
          samples.Length - 1);

      int endSample = Math.Clamp(
          (int)Math.Ceiling(sampleClip.Right - DrawOffset) + 1,
          firstSample + 1,
          samples.Length);

      PointF[] points = new PointF[endSample - firstSample];

      for (int sampleIndex = firstSample; sampleIndex < endSample; sampleIndex++)
      {
        float value = Math.Clamp(samples[sampleIndex] * Volume, -1f, 1f);
        float x = DrawOffset + sampleIndex;
        float y = centerY - value * amplitude;

        points[sampleIndex - firstSample] = new PointF(x, y);
      }

      if (points.Length > 1)
      {
        g.DrawLines(waveformPen, points);
      }

      return;
    }

    // When zoomed out, draw the minimum-to-maximum range for each pixel.
    int totalColumns = (samples.Length + samplesPerPixel - 1) / samplesPerPixel;
    RectangleF waveformClip = g.VisibleClipBounds;

    int firstColumn = Math.Clamp(
        (int)Math.Floor(waveformClip.Left - DrawOffset),
        0,
        totalColumns);

    int endColumn = Math.Clamp(
        (int)Math.Ceiling(waveformClip.Right - DrawOffset),
        firstColumn,
        totalColumns);

    for (int column = firstColumn; column < endColumn; column++)
    {
      int startIndex = column * samplesPerPixel;
      int endIndex = Math.Min(startIndex + samplesPerPixel, samples.Length);

      float minimum = 1f;
      float maximum = -1f;

      for (int i = startIndex; i < endIndex; i++)
      {
        float value = Math.Clamp(samples[i] * Volume, -1f, 1f);
        minimum = Math.Min(minimum, value);
        maximum = Math.Max(maximum, value);
      }

      float x = DrawOffset + column;
      float topY = centerY - maximum * amplitude;
      float bottomY = centerY - minimum * amplitude;

      g.DrawLine(waveformPen, x, topY, x, bottomY);
    }
  }
  public void DrawTrackHeader(Graphics g)
    {
      if (PanDrawSamples is null)
      {
        return;
      }

      System.Drawing.Color trackColor = PanDrawSamples.BackColor;
      System.Drawing.Color headerColor = System.Drawing.Color.FromArgb(
        (int)(trackColor.R * 0.90),
        (int)(trackColor.G * 0.90),
        (int)(trackColor.B * 0.90)
      );

      using SolidBrush headerBrush = new(headerColor);
      using Pen divider = new(System.Drawing.Color.FromArgb(20, 0, 0, 0));
      using System.Drawing.Font nameFont = new("Arial", 10f, System.Drawing.FontStyle.Bold);

      g.FillRectangle(headerBrush, 0, 0, PanDrawSamples.Width, 30);
      g.DrawLine(divider, 0, 29, PanDrawSamples.Width, 29);

      TextRenderer.DrawText(
        g,
        DisplayName,
        nameFont,
        new Rectangle(10, 0, Math.Max(0, PanDrawSamples.Width - 20), 30),
        System.Drawing.Color.Black,
        TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    // dispose
    public void Dispose()
      {
          outputDevice?.Dispose();
          fileReader?.Dispose();
      }
}


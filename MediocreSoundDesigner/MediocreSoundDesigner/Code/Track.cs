using Microsoft.VisualBasic.Devices;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using NAudio.Wave.SampleProviders;

namespace MediocreSoundDesigner;
/// <summary>
/// Used to hold the selection options for the regions of the audio.
/// </summary>
public enum SampleRegion {
    /// <summary>
    /// Only selects the beginningg.
    /// </summary>
    BEGINNING_ONLY,
    /// <summary>
    /// Only selects the end
    /// </summary>
    END_ONLY,
    /// <summary>
    /// Selects the beginning and the end of the sample.
    /// </summary>
    BEGINNING_AND_END,
    /// <summary>
    /// Grabs the entire audio file.
    /// </summary>
    ANYWHERE,
}
/// <summary>
/// Represents one piece of audio: its samples, volume, and drawing position.
/// </summary>
public class Track : IDisposable {
  // Fields
  private string file;
  private TimeSpan duration;
  private float offset;
  private float timeScale;
  private int sampleRate;
  private float[] samples;
  private float pitch;
  private int bits;
  private int channels;
  private AudioFileReader fileReader;
  private WaveOut outputDevice;
  private Panel panContainer;
  private int drawTimescale;

    // properties
    /// <summary>
    /// Offset for how far the track is shifted over to the right in pixels
    /// </summary>
    public int DrawOffset { get; set; }
  /// <summary>
  /// Wether or not the track is currently muted
  /// </summary>
  public bool IsDisabled { get; private set; }

  /// <summary>
  /// Panel that the waveforms of the track are drawn on.
  /// </summary>
  public Panel PanDrawSamples { get; private set; }
  /// <summary>
  /// Represents how loud the sample is
  /// </summary>
  public float Volume { get; private set; }
  private string tempFilePath => Path.Combine(Path.GetTempPath(), "MediocreSoundDesigner", "temp.wav");
  /// <summary>
  /// Wether the track is playing or not
  /// </summary>
  public PlaybackState State => outputDevice?.PlaybackState ?? PlaybackState.Stopped;

    // ctor
    /// <summary>
    /// The file that stores the audio
    /// </summary>
    public Track(string file)
    {
        fileReader = new AudioFileReader(file);
        ISampleProvider provider = fileReader;
        if (provider.WaveFormat.Channels == 2)
            provider = new StereoToMonoSampleProvider(provider);
        if (provider.WaveFormat.SampleRate != 44100)
            provider = new WdlResamplingSampleProvider(provider, 44100);

        List<float> all = new();
        float[] buf = new float[44100];
        int n;
        while ((n = provider.Read(buf)) > 0)
            all.AddRange(buf.AsSpan(0, n).ToArray());
        samples = all.ToArray();

        this.file = file;
        this.duration = fileReader.TotalTime;
        this.offset = 0;
        this.timeScale = 1.0f;
        this.Volume = 1.0f;
        this.pitch = 1.0f;
        this.bits = 32;
        this.channels = 1;
        this.sampleRate = 44100;
        IsDisabled = false;
    }
    /// <summary>
    /// Generates audio instead of pulling it from an external source
    /// </summary>
    public Track(float[] samples) {
    this.samples = samples;

    this.file = "";
    this.duration = TimeSpan.Zero;
    this.offset = 0;
    this.timeScale = 1.0f;
    this.Volume = 1.0f;
    this.pitch = 1.0f;
    this.bits = 32;
    this.channels = 1;
    this.sampleRate = 0;
    IsDisabled = false;
  }
    /// <summary>
    /// Links the track the panel that the track's waveform is drawn over,
    /// and the container that holds all of the track panels
    /// </summary>
    /// <param name="panDrawSamples">The panel that the track's waveform is drawn over.</param>
    /// <param name="panContainer">Container that holds all of the track panels</param>
  public void LinkPanels(Panel panDrawSamples, Panel panContainer) {
    this.PanDrawSamples = panDrawSamples;
    this.panContainer = panContainer;
  }

  // edits
  /// <summary>
  /// Creates a copy of the track
  /// </summary>
  /// <returns>The clone of the sample</returns>
  public float[] CloneSamples() {
    return (float[])samples.Clone();
  }
  /// <summary>
  /// Replaces all of the samples with new samples
  /// </summary>
  /// <param name="samples">The new track sample</param>
  public void ReplaceSamples(float[] samples) {
    this.samples = samples;
  }
    /// <summary>
    /// Reverses the audio file
    /// </summary>
    public void Reverse() {
    for (int i = 0; i < samples.Length / 2; i++) {
      var temp = samples[i];
      samples[i] = samples[samples.Length - 1 - i];
      samples[samples.Length - 1 - i] = temp;
    }
  }
    /// <summary>
    /// Negates the value of each sample (ex: 0.5 becomes -0.5 and -0.5 becomes 0.5)
    /// </summary>
    public void Flip() {
    for (int i = 0; i < samples.Length; i++) {
      samples[i] *= -1;
    }
  }
    /// <summary>
    /// Cuts down audio
    /// </summary>
    public void Trim() {
  }
    /// <summary>
    /// Changing the tracks speed
    /// </summary>
    /// <param name="scale">The speed scale on the track</param>
  public void AlterTimeScale(float scale) {
  }
    /// <summary>
    /// Sets the volume to the given newVolume
    /// </summary>
    public void SetVolume(float newVolume) {
        Volume = newVolume;
    }
    /// <summary>
    /// Changes the pitch by resampling. A factor above 1 raises the pitch (and shortens the track).
    /// </summary>
    /// <param name="factor">Playback-rate multiplier, e.g. 2.0 = one octave up</param>
    public void AlterPitch(float factor)
    {
        if (factor <= 0) return;
        int ch = Math.Max(1, channels);
        int frames = samples.Length / ch;
        if (frames < 2) return;

        int newFrames = (int)(frames / factor);
        float[] result = new float[newFrames * ch];
        for (int f = 0; f < newFrames; f++)
        {
            float src = f * factor;
            int i0 = (int)src;
            int i1 = Math.Min(i0 + 1, frames - 1);
            float t = src - i0;
            for (int c = 0; c < ch; c++)
            {
                result[f * ch + c] = samples[i0 * ch + c] * (1 - t) + samples[i1 * ch + c] * t;
            }
        }
        samples = result;
        pitch *= factor;
    }
    /// <summary>
    /// Eliminates silence algorithmically based on the selected region and thres
    /// </summary>
    public void EliminateSilence(SampleRegion region, float minDuration, float thres) {
    float[] newSamples = CloneSamples();
    if (region is SampleRegion.BEGINNING_ONLY or SampleRegion.BEGINNING_AND_END) {
      int silentDurationInNumSamples = 0;
      for (int i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
        newSamples = newSamples[silentDurationInNumSamples..];
      }
    }
    if (region is SampleRegion.END_ONLY or SampleRegion.BEGINNING_AND_END) {
      int silentDurationInNumSamples = 0;
      for (int i = newSamples.Length - 1; i >= 0; i--) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          break;
        }
      }
      if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
        newSamples = newSamples[0..^silentDurationInNumSamples];
      }
    }
    if (region is SampleRegion.ANYWHERE) {
      int silentDurationInNumSamples = 0;
      int silentRegionStart = 0;
      int i;
      for (i = 0; i < newSamples.Length; i++) {
        if (Math.Abs(newSamples[i]) <= thres) {
          silentDurationInNumSamples++;
        }
        else {
          if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
            newSamples = [..newSamples[0..silentRegionStart], ..newSamples[i..]];
            i = 0;
          }
          silentRegionStart = i + 1;
          silentDurationInNumSamples = 0;
        }
      }
      if ((float)silentDurationInNumSamples / sampleRate >= minDuration) {
        newSamples = [.. newSamples[0..silentRegionStart], .. newSamples[i..]];
      }
    }
    samples = newSamples;
  }

    // play
    /// <summary>
    /// Plays the audio sample
    /// </summary>
    public void Play() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
    SaveToTempFile();
    fileReader = new AudioFileReader(tempFilePath);
    outputDevice = new WaveOut();
    outputDevice.Init(fileReader);
    outputDevice.Play();
  }
    /// <summary>
    /// Pauses the audio sample
    /// </summary>
    public void Pause() {
    outputDevice?.Pause();
  }
    /// <summary>
    /// Resets the audio sample
    /// </summary>
    public void Reset() {
  }
  /// <summary>
  /// Grabs the playhead position
  /// </summary>
  /// <returns>The playhead position as the number of samples</returns>
  public int GetPlayheadPos() {
    return (int)((outputDevice?.GetPosition() ?? 0) / 4);
  }
    /// <summary>
    /// Switches the true/false value of IsDisabled
    /// </summary>
    public void ToggleDisabled() {
    IsDisabled = !IsDisabled;
  }

  // file i/o
  private void SaveToTempFile() {
    Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
    using var writer = new WaveFileWriter(tempFilePath, fileReader?.WaveFormat ?? WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
    writer.WriteSamples(GetAdjustedSamples(), 0, samples.Length);
  }
    /// <summary>
    /// Saves the track to a hard written file
    /// </summary>
    public void SaveToFile(string path) {
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    using var writer = new WaveFileWriter(path, fileReader?.WaveFormat ?? WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
    writer.WriteSamples(GetAdjustedSamples(), 0, samples.Length);
  }
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
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
  /// <summary>
  /// Moves the sample
  /// </summary>
  /// <param name="amount">The amount moved</param>
  public void ShiftSampleOver(int amount) {
    DrawOffset += amount;
    DrawOffset = Math.Max(0, DrawOffset);
  }
  /// <summary>
  /// Draws the strack's waveform onto its panel
  /// </summary>
  /// <param name="g">Graphics surface to draw on.</param>
  /// <param name="drawTimescale">How far along the sample is</param>
  public void DrawSamples(Graphics g, int drawTimescale) {
    if (PanDrawSamples is null) {
      return;
    }

    this.drawTimescale = drawTimescale;
    float heightAdj = PanDrawSamples.Height / 2;
    if (samples != null && samples.Length > 0) {
      int x = DrawOffset;
      Pen pen = new(IsDisabled ? Settings.Color.TrackSamples_Disabled : Settings.Color.TrackSamples_Enabled);
      List<Point> points = new();
      for (int s = 0; s < samples.Length; s += drawTimescale) {
        var sample = (int)(samples[s] * heightAdj * Volume);
        points.Add(new(x, PanDrawSamples.Height / 2));
        points.Add(new(x, PanDrawSamples.Height / 2 - sample));
        x++;
      }
      g.DrawLines(pen, points.ToArray());
      pen.Dispose();
    }
  }

    // dispose
    /// <summary>
    /// Releases the audio output device and file reader used by this track.
    /// </summary>
    public void Dispose() {
    outputDevice?.Dispose();
    fileReader?.Dispose();
  }
}

namespace MediocreSoundDesigner;

public class ProjectData
{
  // Identifies the project file format version
  public int Version { get; set; } = 1;

  // Preserves the waveform's horizontal scale
  public int DrawTimescale { get; set; } = 1;

  // List order preserves the order of the tracks
  public List<TrackData> Tracks { get; set; } = new();
}

public class TrackData
{
  // Raw audio, before applying volume or timeline positioning
  public float[] Samples { get; set; } = Array.Empty<float>();

  // Audio format information
  public int SampleRate { get; set; }
  public int Channels { get; set; }

  // Track settings
  public float Volume { get; set; } = 1.0f;
  public int DrawOffset { get; set; }
  public bool IsDisabled { get; set; }
  public string DisplayName { get; set; } = "Generated Track";
  public int? TrackColorArgb { get; set; }
}
using MeltySynth;
using NAudio.Wave;

namespace MediocreSoundDesigner;

public static class SoundFontGenerator
{
  private const int SampleRate = 44100;
  private const double ReleaseSeconds = 0.75;

  public static Track CreateTestTrack()
  {
    string soundFontPath = Path.Combine(
      AppContext.BaseDirectory,
      "SoundFonts",
      "GeneralUser GS SoftSynth v1.44.sf2");

    if (!File.Exists(soundFontPath))
    {
      throw new FileNotFoundException("The SoundFont file was not found.", soundFontPath);
    }

    Synthesizer synthesizer = new(soundFontPath, SampleRate);

    float[] left = new float[SampleRate * 2];
    float[] right = new float[SampleRate * 2];

    synthesizer.NoteOn(0, 60, 100);
    synthesizer.Render(left.AsSpan(0, SampleRate), right.AsSpan(0, SampleRate));

    synthesizer.NoteOff(0, 60);
    synthesizer.Render(left.AsSpan(SampleRate), right.AsSpan(SampleRate));

    float[] samples = new float[left.Length * 2];

    for (int i = 0; i < left.Length; i++)
    {
      samples[i * 2] = left[i];
      samples[i * 2 + 1] = right[i];
    }

    WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 2);
    return new Track(samples, format);
  }

  public static Track CreateChordTrack(string rootNote, string chordType, int octave, double durationSeconds)
  {
    string soundFontPath = Path.Combine(
      AppContext.BaseDirectory,
      "SoundFonts",
      "GeneralUser GS SoftSynth v1.44.sf2");

    if (!File.Exists(soundFontPath))
    {
      throw new FileNotFoundException("The SoundFont file was not found.", soundFontPath);
    }

    string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    int rootIndex = Array.IndexOf(noteNames, rootNote.Trim());

    if (rootIndex < 0)
    {
      throw new InvalidDataException($"Unknown root note: {rootNote}");
    }

    int[] intervals = chordType.Trim().ToLowerInvariant() switch
    {
      "major" or "major triad" or "major chord" => new[] { 0, 4, 7 },
      "minor" or "minor triad" or "minor chord" => new[] { 0, 3, 7 },
      "diminished" or "diminished triad" or "diminished chord" => new[] { 0, 3, 6 },
      "augmented" or "augmented triad" or "augmented chord" => new[] { 0, 4, 8 },
      _ => throw new InvalidDataException($"Unknown chord type: {chordType}")
    };

    Synthesizer synthesizer = new(soundFontPath, SampleRate);
    int frameCount = Math.Max(1, (int)Math.Round(durationSeconds * SampleRate));
    int releaseFrames = (int)Math.Round(SampleRate * ReleaseSeconds);
    float[] left = new float[frameCount + releaseFrames];
    float[] right = new float[frameCount + releaseFrames];

    int rootMidiNote = 12 * (octave + 1) + rootIndex;
    int[] notes = intervals.Select(interval => rootMidiNote + interval).ToArray();

    foreach (int note in notes)
    {
      synthesizer.NoteOn(0, note, 100);
    }

    synthesizer.Render(left.AsSpan(0, frameCount), right.AsSpan(0, frameCount));

    foreach (int note in notes)
    {
      synthesizer.NoteOff(0, note);
    }

    synthesizer.Render(left.AsSpan(frameCount, releaseFrames), right.AsSpan(frameCount, releaseFrames));
    ApplyReleaseFade(left, right, frameCount, releaseFrames);

    float[] samples = new float[left.Length * 2];

    for (int i = 0; i < left.Length; i++)
    {
      samples[i * 2] = Math.Clamp(left[i], -1f, 1f);
      samples[i * 2 + 1] = Math.Clamp(right[i], -1f, 1f);
    }

    return new Track(samples, WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 2));
  }

  public static Track CreatePluckTrack(string instrument, string noteName, int octave, double durationSeconds)
  {
    string soundFontPath = Path.Combine(
      AppContext.BaseDirectory,
      "SoundFonts",
      "GeneralUser GS SoftSynth v1.44.sf2");

    if (!File.Exists(soundFontPath))
    {
      throw new FileNotFoundException("The SoundFont file was not found.", soundFontPath);
    }

    string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    int noteIndex = Array.IndexOf(noteNames, noteName.Trim());

    if (noteIndex < 0)
    {
      throw new InvalidDataException($"Unknown note: {noteName}");
    }

    int program = instrument.Trim() switch
    {
      "Acoustic Guitar" => 24,
      "Electric Guitar" => 27,
      _ => throw new InvalidDataException($"Unknown instrument: {instrument}")
    };

    Synthesizer synthesizer = new(soundFontPath, SampleRate);
    synthesizer.ProcessMidiMessage(0, 0xC0, program, 0);

    int frameCount = Math.Max(1, (int)Math.Round(durationSeconds * SampleRate));
    int releaseFrames = (int)Math.Round(SampleRate * ReleaseSeconds);
    float[] left = new float[frameCount + releaseFrames];
    float[] right = new float[frameCount + releaseFrames];

    int midiNote = 12 * (octave + 1) + noteIndex;

    synthesizer.NoteOn(0, midiNote, 90);
    synthesizer.Render(left.AsSpan(0, frameCount), right.AsSpan(0, frameCount));
    synthesizer.NoteOff(0, midiNote);
    synthesizer.Render(left.AsSpan(frameCount, releaseFrames), right.AsSpan(frameCount, releaseFrames));
    ApplyReleaseFade(left, right, frameCount, releaseFrames);

    float[] samples = new float[left.Length * 2];

    for (int i = 0; i < left.Length; i++)
    {
      samples[i * 2] = Math.Clamp(left[i], -1f, 1f);
      samples[i * 2 + 1] = Math.Clamp(right[i], -1f, 1f);
    }

    return new Track(samples, WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 2));
  }

  public static Track CreateBeatTrack(string drumKit, string rhythm, int tempo, int bars)
  {
    string soundFontPath = Path.Combine(
      AppContext.BaseDirectory,
      "SoundFonts",
      "GeneralUser GS SoftSynth v1.44.sf2");

    if (!File.Exists(soundFontPath))
    {
      throw new FileNotFoundException("The SoundFont file was not found.", soundFontPath);
    }

    int kitProgram = drumKit.Trim() switch
    {
      "Standard" => 0,
      "Room" => 8,
      "Electronic" => 24,
      "Jazz" => 32,
      "Brush" => 40,
      "SFX" => 56,
      _ => throw new InvalidDataException($"Unknown drum kit: {drumKit}")
    };

    Synthesizer synthesizer = new(soundFontPath, SampleRate);
    synthesizer.ProcessMidiMessage(9, 0xC0, kitProgram, 0);

    int totalSteps = bars * 8;
    int stepFrames = (int)Math.Round(SampleRate * 60d / tempo / 2d);
    int musicFrames = totalSteps * stepFrames;
    int releaseFrames = (int)Math.Round(SampleRate * ReleaseSeconds);

    float[] left = new float[musicFrames + releaseFrames];
    float[] right = new float[musicFrames + releaseFrames];
    int cursor = 0;

    for (int step = 0; step < totalSteps; step++)
    {
      List<(int Note, int Velocity)> hits = GetBeatHits(rhythm, step % 8);

      foreach ((int note, int velocity) in hits)
      {
        synthesizer.NoteOn(9, note, velocity);
      }

      int hitFrames = hits.Count == 0 ? 0 : Math.Min(SampleRate / 20, stepFrames);

      if (hitFrames > 0)
      {
        synthesizer.Render(left.AsSpan(cursor, hitFrames), right.AsSpan(cursor, hitFrames));
        cursor += hitFrames;

        foreach ((int note, int _) in hits)
        {
          synthesizer.NoteOff(9, note);
        }
      }

      int stepEnd = (step + 1) * stepFrames;
      int remainingFrames = stepEnd - cursor;

      if (remainingFrames > 0)
      {
        synthesizer.Render(left.AsSpan(cursor, remainingFrames), right.AsSpan(cursor, remainingFrames));
        cursor = stepEnd;
      }
    }

    synthesizer.Render(left.AsSpan(musicFrames, releaseFrames), right.AsSpan(musicFrames, releaseFrames));
    ApplyReleaseFade(left, right, musicFrames, releaseFrames);

    float[] samples = new float[left.Length * 2];

    for (int i = 0; i < left.Length; i++)
    {
      samples[i * 2] = Math.Clamp(left[i], -1f, 1f);
      samples[i * 2 + 1] = Math.Clamp(right[i], -1f, 1f);
    }

    return new Track(samples, WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 2));
  }

  private static void ApplyReleaseFade(float[] left, float[] right, int startFrame, int releaseFrames)
  {
    if (releaseFrames <= 1)
    {
      if (releaseFrames == 1)
      {
        left[startFrame] = 0f;
        right[startFrame] = 0f;
      }
      return;
    }

    for (int frame = 0; frame < releaseFrames; frame++)
    {
      float gain = 1f - frame / (float)(releaseFrames - 1);
      int index = startFrame + frame;
      left[index] *= gain;
      right[index] *= gain;
    }
  }

  private static List<(int Note, int Velocity)> GetBeatHits(string rhythm, int step)
  {
    List<(int Note, int Velocity)> hits = new();

    void Add(int note, int velocity)
    {
      hits.Add((note, velocity));
    }

    switch (rhythm.Trim().ToLowerInvariant())
    {
      case "rock":
        if (step == 0 || step == 4) Add(36, 100);
        if (step == 2 || step == 6) Add(38, 95);
        Add(42, 60);
        break;

      case "pop":
        if (step == 0 || step == 4 || step == 5) Add(36, 95);
        if (step == 2 || step == 6) Add(38, 90);
        if (step == 7) Add(46, 65);
        else if (step % 2 == 0) Add(42, 55);
        break;

      case "hip-hop":
        if (step == 0 || step == 5) Add(36, 100);
        if (step == 4) Add(38, 95);
        if (step % 2 == 0) Add(42, 60);
        break;

      case "four on the floor":
        if (step % 2 == 0) Add(36, 100);
        if (step == 2 || step == 6) Add(38, 90);
        Add(42, 60);
        break;

      default:
        throw new InvalidDataException($"Unknown rhythm: {rhythm}");
    }

    return hits;
  }
}
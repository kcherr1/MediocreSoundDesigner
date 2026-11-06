using NAudio.Wave;
using NAudio.SoundFile;

namespace MediocreSoundDesigner; 

public class TrackManager {
  private Track pseudoCombinedTrack;
  private Panel panContainer;

  public List<Track> Tracks { get; private set; }
	public Track SelectedTrack { get; private set; }
  public int DrawTimescale { get; set; }
  public PlaybackState State => pseudoCombinedTrack?.State ?? PlaybackState.Stopped;

  public TrackManager(Panel panContainer) {
		Tracks = new();
    SelectedTrack = null;
    pseudoCombinedTrack = null;
    this.panContainer = panContainer;
  }

	public void AddTrack(Track track) {
		Tracks.Add(track);
	}

	public void RemTrack(Track track) {
		Tracks.Remove(track);
	}

	public void SelectTrack(Track track) {
		SelectedTrack = track;
    Tracks.ForEach(track => {
      track.PanDrawSamples.BorderStyle = BorderStyle.None;
      track.PanDrawSamples.BackColor = Settings.Color.TrackBg_Unselected;
    });
    track.PanDrawSamples.BorderStyle = BorderStyle.Fixed3D;
    track.PanDrawSamples.BackColor = Settings.Color.TrackBg_Selected;
  }

  public void RedrawAll() {
    Tracks.ForEach(track => track.PanDrawSamples.Invalidate());
  }

  public void ApplySettings() {
    foreach (Track track in Tracks) {
      if (SelectedTrack == track) {
        track.PanDrawSamples.BackColor = Settings.Color.TrackBg_Selected;
      }
      else {
        track.PanDrawSamples.BackColor = Settings.Color.TrackBg_Unselected;
      }
    }
    RedrawAll();
  }

    public void SaveToFile(string filename)
    {
        CombineTracks();

        // Get the combined audio samples
        ISampleProvider source = new FloatArraySampleProvider(
            pseudoCombinedTrack.GetAdjustedSamples(),
            pseudoCombinedTrack.Format
        );

        // Convert the samples into a format the encoder accepts
        IWaveProvider waveSource = source.ToWaveProvider();

        // Export using the file type specified
        SoundFileWriter.CreateSoundFile(filename, waveSource);
    }

    public void PlayAll() {
    CombineTracks();
    pseudoCombinedTrack.Play();
  }
    private void CombineTracks()
    {
        pseudoCombinedTrack?.Dispose();

        List<float> combinedSamples = new();

        // Find the first ENABLED track and use it for the output format
        Track referenceTrack = Tracks.Find(track => !track.IsDisabled);
        WaveFormat outputFormat = referenceTrack is null
            ? WaveFormat.CreateIeeeFloatWaveFormat(44100, 1)
            : WaveFormat.CreateIeeeFloatWaveFormat(
                referenceTrack.Format.SampleRate,
                referenceTrack.Format.Channels
            );

        foreach (Track track in Tracks)
        {
            if (track.IsDisabled)
            {
                continue;
            }

            // Convert this track to match the output format, then add to the combined
            float[] trackSamples = track.GetSamplesInFormat(outputFormat);
            for (int i = 0; i < trackSamples.Length; i++)
            {
                if (combinedSamples.Count <= i)
                {
                    combinedSamples.Add(0);
                }
                combinedSamples[i] += trackSamples[i];
            }
        }

        // Combine the tracks into a new one
        pseudoCombinedTrack = new(combinedSamples.ToArray(), outputFormat);
        pseudoCombinedTrack.LinkPanels(null, panContainer);
    }
    public void PauseAll() {
    pseudoCombinedTrack?.Pause();
  }
  public int GetPlayheadPos() {
    return (pseudoCombinedTrack?.GetPlayheadPos() ?? 0) / DrawTimescale;
  }
}

using NAudio.Wave;
using System.Text.Json;

namespace MediocreSoundDesigner;

public class TrackManager {
    private Track pseudoCombinedTrack;
    private Panel panContainer;
    private int nextTrackColorIndex = 0;

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
        if (track.TrackColor.IsEmpty) {
            int index = nextTrackColorIndex % Settings.DefaultTrackColors.Length;
            track.SetTrackColor(Settings.DefaultTrackColors[index]);
        }

        nextTrackColorIndex++;
        Tracks.Add(track);
    }

    public void RemTrack(Track track) {
        Tracks.Remove(track);
    }

    public void SelectTrack(Track track) {
        SelectedTrack = track;

        foreach (Track other in Tracks) {
            other.PanDrawSamples.BorderStyle = BorderStyle.None;
            other.PanDrawSamples.BackColor = other.TrackColor.IsEmpty
                ? Settings.Color.TrackBg_Unselected
                : other.TrackColor;
        }

        track.PanDrawSamples.BorderStyle = BorderStyle.Fixed3D;
        track.PanDrawSamples.BackColor = track.TrackColor.IsEmpty
            ? Settings.Color.TrackBg_Selected
            : track.TrackColor;
    }

    public void RedrawAll() {
        Tracks.ForEach(track => track.PanDrawSamples.Invalidate());
    }

    public void ApplySettings() {
        foreach (Track track in Tracks) {
            track.PanDrawSamples.BackColor = track.TrackColor.IsEmpty
                ? Settings.Color.TrackBg_Unselected
                : track.TrackColor;
        }

        RedrawAll();
    }

    public void SaveToFile(string filename) {
        CombineTracks();
        pseudoCombinedTrack.SaveToFile(filename);
    }

    public void SaveProject(string filename) {
        ProjectData project = new() {
            DrawTimescale = DrawTimescale
        };

        foreach (Track track in Tracks) {
            project.Tracks.Add(track.GetProjectData());
        }

        string fullPath = Path.GetFullPath(filename);
        string tempPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";

        try {
            using (FileStream stream = new(tempPath, FileMode.CreateNew, FileAccess.Write)) {
                JsonSerializer.Serialize(stream, project);
            }

            File.Move(tempPath, fullPath, overwrite: true);
        }
        finally {
            if (File.Exists(tempPath)) {
                File.Delete(tempPath);
            }
        }
    }

    public void LoadProject(string filename) {
        ProjectData project;

        using (FileStream stream = File.OpenRead(filename)) {
            project = JsonSerializer.Deserialize<ProjectData>(stream);
        }

        if (project is null || project.Version != 1 || project.Tracks is null || project.DrawTimescale <= 0) {
            throw new InvalidDataException("This project file is invalid or unsupported.");
        }

        List<Track> loadedTracks = new();

        foreach (TrackData data in project.Tracks) {
            if (data is null || data.Samples is null || data.SampleRate < 0 || data.Channels <= 0 ||
                data.DrawOffset < 0 || !float.IsFinite(data.Volume) || data.Volume < 0) {
                throw new InvalidDataException("The project contains invalid track settings.");
            }

            if (data.Samples.Length % data.Channels != 0 || data.Samples.Any(sample => !float.IsFinite(sample))) {
                throw new InvalidDataException("The project contains invalid audio data.");
            }

            loadedTracks.Add(new Track(data));
        }

        pseudoCombinedTrack?.Dispose();
        pseudoCombinedTrack = null;

        foreach (Track track in Tracks) {
            track.PanDrawSamples?.Dispose();
            track.Dispose();
        }

        Tracks = loadedTracks;
        SelectedTrack = null;
        DrawTimescale = project.DrawTimescale;
    }

    public void NewProject() {
        pseudoCombinedTrack?.Dispose();
        pseudoCombinedTrack = null;

        foreach (Track track in Tracks) {
            track.PanDrawSamples?.Dispose();
            track.Dispose();
        }

        Tracks.Clear();
        SelectedTrack = null;
    }

  public void PlayAll()
  {
    if (pseudoCombinedTrack?.State == PlaybackState.Paused)
    {
      pseudoCombinedTrack.Resume();
      return;
    }

    CombineTracks();
    pseudoCombinedTrack.Play();
  }

  private void CombineTracks() {
        pseudoCombinedTrack?.Dispose();
        List<float> combinedSamples = new();

        foreach (Track track in Tracks) {
            if (track.IsDisabled) {
                continue;
            }

            float[] trackSamples = track.GetAdjustedSamples();

            for (int i = 0; i < trackSamples.Length; i++) {
                if (combinedSamples.Count <= i) {
                    combinedSamples.Add(0);
                }

                combinedSamples[i] += trackSamples[i];
            }
        }

        Track first = Tracks.FirstOrDefault(track => !track.IsDisabled);
        float[] mixed = combinedSamples.Select(sample => Math.Clamp(sample, -1f, 1f)).ToArray();

        pseudoCombinedTrack = first is null
            ? new Track(mixed)
            : new Track(mixed, first.Format);

        pseudoCombinedTrack.LinkPanels(null, panContainer);
    }

    public void PauseAll() {
        pseudoCombinedTrack?.Pause();
    }

    public void ResetAll() {
        pseudoCombinedTrack?.Dispose();
        pseudoCombinedTrack = null;
    }

    public void ResumeAll() {
        pseudoCombinedTrack?.Resume();
    }

    public int GetPlayheadPos() {
        return (pseudoCombinedTrack?.GetPlayheadPos() ?? 0) / DrawTimescale;
    }
}
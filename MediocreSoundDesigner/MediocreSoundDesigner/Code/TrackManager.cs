using NAudio.Wave;

namespace MediocreSoundDesigner;

public class TrackManager
{
    private Track pseudoCombinedTrack;
    private Panel panContainer;
    public List<Track> Tracks { get; private set; }
    public List<Track> SelectedTracks { get; } = new();
    public Track SelectedTrack => SelectedTracks.FirstOrDefault();
    public int? SelectionStartX { get; private set; }
    public int? SelectionEndX { get; private set; }
    public bool HasSelection => SelectionStartX.HasValue && SelectionEndX.HasValue;
    public const int MIN_DRAW_TIMESCALE = 1;
    public const int MAX_DRAW_TIMESCALE = 200;

    // what the slider asked for; the real value is clamped when read
    private int requestedDrawTimescale = MIN_DRAW_TIMESCALE;
    public int DrawTimescale
    {
        get => Math.Clamp(requestedDrawTimescale, MIN_DRAW_TIMESCALE, MAX_DRAW_TIMESCALE);
        set => requestedDrawTimescale = value;
    }

    public PlaybackState State => pseudoCombinedTrack?.State ?? PlaybackState.Stopped;

    public TrackManager(Panel panContainer)
    {
        Tracks = new();
        pseudoCombinedTrack = null;
        this.panContainer = panContainer;
    }

    public void AddTrack(Track track)
    {
        Tracks.Add(track);
    }

    public void RemTrack(Track track)
    {
        Tracks.Remove(track);
        SelectedTracks.Remove(track);
    }

    public void SelectTrack(Track track, bool toggle = false)
    {
        if (!toggle)
        {
            SelectedTracks.Clear();
            SelectedTracks.Add(track);
        }
        else
        {
            if (SelectedTracks.Contains(track))
            {
                SelectedTracks.Remove(track);
            }
            else
            {
                SelectedTracks.Add(track);
            }
        }

        // Synchronize sample selection bounds with the active tracks
        foreach (Track t in Tracks)
        {
            if (!SelectedTracks.Contains(t))
            {
                t.ClearSelection();
            }
            else if (HasSelection)
            {
                int s1 = (SelectionStartX.Value - t.DrawOffset) * DrawTimescale;
                int s2 = (SelectionEndX.Value - t.DrawOffset) * DrawTimescale;
                t.SetSelection(s1, s2);
            }
        }

        ApplySelectionStyles();
    }

    public void SetSelection(int startX, int endX)
    {
        SelectionStartX = Math.Min(startX, endX);
        SelectionEndX = Math.Max(startX, endX);

        foreach (Track track in SelectedTracks)
        {
            int s1 = (SelectionStartX.Value - track.DrawOffset) * DrawTimescale;
            int s2 = (SelectionEndX.Value - track.DrawOffset) * DrawTimescale;
            track.SetSelection(s1, s2);
        }

        // Force all selected tracks to paint together in lockstep
        RedrawSelected();
    }

    public void SelectAllTracks()
    {
        SelectedTracks.Clear();
        SelectedTracks.AddRange(Tracks);
        ApplySelectionStyles();
    }

    public void ApplySelectionStyles()
    {
        foreach (Track track in Tracks)
        {
            if (track.PanDrawSamples is null) continue;
            bool isSelected = SelectedTracks.Contains(track);
            track.PanDrawSamples.BorderStyle = isSelected ? BorderStyle.Fixed3D : BorderStyle.None;
            track.PanDrawSamples.BackColor = isSelected ? Settings.Color.TrackBg_Selected : Settings.Color.TrackBg_Unselected;
        }
    }

    public void RedrawSelected()
    {
        foreach (Track track in SelectedTracks)
        {
            track.PanDrawSamples?.Refresh();
        }
    }

    public void RedrawAll()
    {
        foreach (Track track in Tracks)
        {
            track.PanDrawSamples?.Invalidate();
        }
    }
    public void ApplySettings()
    {
        ApplySelectionStyles();
        RedrawAll();
    }

    public void ClearSelection()
    {
        SelectionStartX = null;
        SelectionEndX = null;

        foreach (Track track in Tracks)
        {
            track.ClearSelection();
            track.PanDrawSamples?.Refresh();
        }
    }

    public void SaveToFile(string filename)
    {
        CombineTracks();
        pseudoCombinedTrack.SaveToFile(filename);
    }

    public void PlayAll()
    {
        CombineTracks();
        pseudoCombinedTrack.Play();
    }

    private void CombineTracks()
    {
        pseudoCombinedTrack?.Dispose();
        List<float> combinedSamples = new();
        foreach (Track track in Tracks)
        {
            if (track.IsDisabled)
            {
                continue;
            }
            float[] trackSamples = (float[])track.GetAdjustedSamples().Clone();
            for (int i = 0; i < trackSamples.Length; i++)
            {
                if (combinedSamples.Count <= i)
                {
                    combinedSamples.Add(0);
                }
                combinedSamples[i] += trackSamples[i];
            }
        }
        pseudoCombinedTrack = new(combinedSamples.ToArray());
        pseudoCombinedTrack.LinkPanels(null, panContainer);
    }

    public void PauseAll()
    {
        pseudoCombinedTrack?.Pause();
    }

    public int GetPlayheadPos()
    {
        return (pseudoCombinedTrack?.GetPlayheadPos() ?? 0) / DrawTimescale;
    }
}

using NAudio.Wave;

namespace MediocreSoundDesigner;

public class TrackManager
{
    private Track pseudoCombinedTrack;
    private Panel panContainer;
    private int currentSample; // remembered playhead position (used while paused/stopped)
    public List<Track> Tracks { get; private set; }
    public int TrackCounter { get; private set; }
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
    public int TotalSamples => Tracks.Where(t => !t.IsDisabled)
        .Select(t => t.AdjustedSampleCount).DefaultIfEmpty(0).Max();
    public int PlayheadSample => Math.Clamp(
        State == PlaybackState.Playing ? pseudoCombinedTrack.GetPlayheadPos() : currentSample,
        0, TotalSamples);
    public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)PlayheadSample / Track.INTERNAL_SAMPLE_RATE);
    public TimeSpan TotalTime => TimeSpan.FromSeconds((double)TotalSamples / Track.INTERNAL_SAMPLE_RATE);

    public TrackManager(Panel panContainer)
    {
        TrackCounter = 0;
        Tracks = new();
        pseudoCombinedTrack = null;
        this.panContainer = panContainer;
    }

    public void AddTrack(Track track)
    {
        TrackCounter++;
        Tracks.Add(track);
    }

    public void RemTrack(Track track)
    {
        Tracks.Remove(track);
        SelectedTracks.Remove(track);
        ClearSelection();
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
            // no 3D border on selection (selection is shown by the background color): the border
            // shifted the waveform a couple of pixels and cut gaps in the playline
            track.PanDrawSamples.BorderStyle = BorderStyle.None;
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
        // always rebuild so edits made while paused are heard, but resume from where we left off
        CombineTracks();
        if (currentSample >= pseudoCombinedTrack.SampleCount)
        {
            currentSample = 0; // previous run finished (or seeked to the end): start over
        }
        pseudoCombinedTrack.Play(currentSample);
    }

    // move the playhead; if audio is playing, jump there immediately
    public void SeekToSample(int sample)
    {
        currentSample = Math.Clamp(sample, 0, TotalSamples);
        if (State == PlaybackState.Playing)
        {
            pseudoCombinedTrack.Seek(currentSample);
        }
    }

    // stop playback (if any) and put the playhead back at the start
    public void ResetPlayhead()
    {
        pseudoCombinedTrack?.Reset();
        currentSample = 0;
    }

    // called when playback runs off the end of the track
    public void OnPlaybackEnded()
    {
        currentSample = TotalSamples;
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
        if (State == PlaybackState.Playing)
        {
            currentSample = pseudoCombinedTrack.GetPlayheadPos(); // remember where we paused
        }
        pseudoCombinedTrack?.Pause();
    }

    public int GetPlayheadPos()
    {
        return PlayheadSample / DrawTimescale;
    }
}
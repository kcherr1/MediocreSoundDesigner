using MediocreSoundDesigner.Properties;
using NAudio.Wave;
using System.Drawing.Printing;
using System.Text.Json;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form
{
    // fields
    private TrackManager trackManager;
    private PlayheadSlider sldPlayhead;
    private bool isScrubbing;
    private int playlineX; // playhead position in content pixels
    private WaveIn waveSource;
    private Track recordingTrack;
    private bool isRecording;
    private List<float> recordedSamples = new();
    private readonly object recordedSamplesLock = new();
    private int renderedRecordingSampleCount;

    // ctor
    public FrmMain()
    {
        InitializeComponent();
        Setup();
    }

    // setup
    private void Setup()
    {
        DoubleBuffered = true;
        panTracks.AutoScroll = true;

        // Enable double-buffering on panTracks container to prevent child clipping flicker
        typeof(Panel).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.SetProperty |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic,
            null, panTracks, new object[] { true });

        trackManager = new(panTracks);

        // draggable playhead slider that sits above the playline
        sldPlayhead = new PlayheadSlider();
        sldPlayhead.Scrubbing += SldPlayhead_Scrubbing;
        sldPlayhead.ScrubCompleted += SldPlayhead_ScrubCompleted;
        panApp.Controls.Add(sldPlayhead);

        // the reset button itself now lives in FrmMain.Designer.cs
        new ToolTip().SetToolTip(bttnReset, "Reset to start");
        // the scroll event fires before the panel has actually moved, so refresh again once it has
        panTracks.Scroll += (s, e) => { RefreshPlayheadVisuals(); BeginInvoke(new Action(RefreshPlayheadVisuals)); };
        panTracks.MouseWheel += (s, e) => BeginInvoke(new Action(RefreshPlayheadVisuals));

        // the playline is painted straight onto the track surfaces (no moving child control),
        // so it can't leave trails and moves in the same paint pass as the waveform
        lblPlayhead.Visible = false;
        panTracks.Paint += (s, e) => DrawPlayline(e.Graphics, panTracks);
        bttnPlay.BackgroundImage = Resources.play;
        lblVolumePercent.Text = $"{trkVolume.Value}%";
        trackManager.DrawTimescale = trkDrawTimescale.Value;
        UpdateSizes();
        ApplySettings();
    }
    private void ApplySettings()
    {
        bttnPlay.BackgroundImage = Resources.play;

        BackColor = Settings.Color.WinBg;
        ForeColor = Settings.Color.WinFg;
        panTopActions.BackColor = Settings.Color.WinBg;
        panTopActions.ForeColor = Settings.Color.WinFg;
        panBottomActions.BackColor = Settings.Color.WinBg;
        panBottomActions.ForeColor = Settings.Color.WinFg;
        menu.ForeColor = Settings.Color.WinFg;
        sldPlayhead.BackColor = Settings.Color.WinBg;
        bttnReset.BackgroundImage = CreateResetIcon(Settings.Color.WinFg);
        sldPlayhead.Invalidate();
        lblDebug.ForeColor = Settings.Color.WinFg;
        panTracks.BackColor = Settings.Color.TrackBg_Unselected;
        lblPlayhead.BackColor = Settings.Color.Playline;
        lblCurrTime.ForeColor = Settings.Color.WinFg;
        lblVolumePercent.ForeColor = Settings.Color.WinFg;

        foreach (Control control in panTracks.Controls)
        {
            if (control is not Panel trackContainer || control == lblPlayhead) continue;

            trackContainer.BackColor = Settings.Color.TrackBg_Unselected;
            foreach (Control child in trackContainer.Controls)
            {
                if (child is DoubleBufferedPanel trackSurface)
                {
                    trackSurface.BackColor = Settings.Color.TrackBg_Unselected;
                }
                else if (child is Label trackName)
                {
                    trackName.ForeColor = Settings.Color.WinFg;
                }
            }
        }

        // slider range comes from the same constants the clamp uses
        trkDrawTimescale.Minimum = TrackManager.MIN_DRAW_TIMESCALE;
        trkDrawTimescale.Maximum = TrackManager.MAX_DRAW_TIMESCALE;

        trackManager.DrawTimescale = GetTimescaleFromSlider();
        trackManager.ApplySettings();
        UpdateSizes();
    }

    private int GetTimescaleFromSlider()
    {
        // far right = most zoomed in = 1 sample per pixel, never 0
        return trkDrawTimescale.Maximum - trkDrawTimescale.Value + trkDrawTimescale.Minimum;
    }

    private void SetDefaultZoom()
    {
        trkDrawTimescale.Value = trkDrawTimescale.Minimum +
            (trkDrawTimescale.Maximum - trkDrawTimescale.Minimum) / 2;
        trackManager.DrawTimescale = GetTimescaleFromSlider();
    }

    private void FitToWindow()
    {
        if (trackManager.Tracks.Count == 0) return;

        int longest = trackManager.Tracks.Max(t => t.SampleCount);
        int fit = (int)Math.Ceiling((double)longest / Math.Max(1, panTracks.ClientSize.Width));
        fit = Math.Clamp(fit, TrackManager.MIN_DRAW_TIMESCALE, TrackManager.MAX_DRAW_TIMESCALE);

        // inverse of GetTimescaleFromSlider
        trkDrawTimescale.Value = trkDrawTimescale.Maximum - fit + trkDrawTimescale.Minimum;
        trackManager.DrawTimescale = fit;
    }

    // GUI controls
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (trackManager.SelectedTracks.Count > 0)
        {
            if (keyData == Keys.Left)
            {
                foreach (Track t in trackManager.SelectedTracks)
                {
                    t.ShiftSampleOver(-5);
                    t.PanDrawSamples.Invalidate();
                }
                return true;
            }
            else if (keyData == Keys.Right)
            {
                foreach (Track t in trackManager.SelectedTracks)
                {
                    t.ShiftSampleOver(+5);
                    t.PanDrawSamples.Invalidate();
                }
                return true;
            }
            else if (keyData == Keys.Delete)
            {
                DeleteTracks(trackManager.SelectedTracks);
                return true;
            }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FrmMain_SizeChanged(object sender, EventArgs e)
    {
        UpdateSizes();
    }

    private void btnPlay_Click(object sender, EventArgs e)
    {
        if (trackManager.State is NAudio.Wave.PlaybackState.Stopped or NAudio.Wave.PlaybackState.Paused)
        {
            trackManager.PlayAll();
            bttnPlay.BackgroundImage = Resources.pause;
            tmrUpdatePlaybackState_Tick(sender, e);
            tmrUpdate.Start();
        }
        else
        {
            trackManager.PauseAll();
            bttnPlay.BackgroundImage = Resources.play;
            if (!isRecording)
            {
                tmrUpdate.Stop();
            }
        }
    }

    private void bttnReset_Click(object sender, EventArgs e)
    {
        tmrUpdate.Stop(); // stop first so the "playback ended" check doesn't move the playhead to the end
        trackManager.ResetPlayhead();
        bttnPlay.BackgroundImage = Resources.play;
        panTracks.AutoScrollPosition = new Point(0, -panTracks.AutoScrollPosition.Y); // scroll back to the left
        RefreshPlayheadVisuals();
        BeginInvoke(new Action(RefreshPlayheadVisuals)); // again once the scroll has been applied
    }

    // "skip to start" icon: a bar and a left-pointing triangle
    private static Bitmap CreateResetIcon(Color color)
    {
        Bitmap bmp = new(64, 64);
        using Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using SolidBrush brush = new(color);
        g.FillRectangle(brush, 10, 10, 8, 44);
        g.FillPolygon(brush, new[] { new Point(54, 10), new Point(54, 54), new Point(22, 32) });
        return bmp;
    }

    private void menuFileOpen_Click(object sender, EventArgs e)
    {
        ofdLoadFile.DefaultExt = "wav";
        ofdLoadFile.Multiselect = true;
        // applies optional filter types in file explorer, Audio Files (both wav and mp3), MP3 Audio, WAV Audio
        ofdLoadFile.Filter = "Audio Files (*.mp3;*.wav)|*.mp3;*.wav|" + "MP3 Audio (*.mp3)|*.mp3|" + "WAV Audio (*.wav)|*.wav";
        if (ofdLoadFile.ShowDialog() == DialogResult.OK)
        {
            foreach (string file in ofdLoadFile.FileNames)
            { // iterate through array of selected files
                ImportTrackFile(file);
            }
        }
    }

    public async Task<Track> GenerateAndImportUdioTrackAsync(
        string prompt,
        bool makeInstrumental = true,
        CancellationToken cancellationToken = default)
    {
        using UdioApiClient udio = new();
        string filePath = await udio.GenerateAndDownloadAsync(
            prompt,
            makeInstrumental,
            cancellationToken: cancellationToken);

        try
        {
            return ImportTrackFile(filePath);
        }
        catch
        {
            File.Delete(filePath);
            throw;
        }
    }

    private Track ImportTrackFile(string filePath)
    {
        Track newTrack = new(filePath);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
        SetDefaultZoom();
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();
        return newTrack;
    }

    private void menuFileSave_Click(object sender, EventArgs e)
    {
        sfdSaveFile.DefaultExt = "wav";
        sfdSaveFile.Filter = "WAV Audio (*.wav)|*.wav|MP3 Audio (*.mp3)|*.mp3";
        sfdSaveFile.FilterIndex = 1;
        sfdSaveFile.AddExtension = true;
        if (sfdSaveFile.ShowDialog() == DialogResult.OK)
        {
            trackManager.SaveToFile(sfdSaveFile.FileName);
        }
    }

    private void menuIOAudioMedia_Click(object sender, EventArgs e)
    {
        FrmAudioMedia frm = new();
        if (frm.ShowDialog() == DialogResult.OK)
        {
            // Handle any actions needed after the dialog is closed with OK
        }
    }

    private void menuFileSettings_Click(object sender, EventArgs e)
    {
        FrmEditSettings frm = new();
        frm.SetSound(trackManager.SelectedTrack);
        if (frm.ShowDialog() == DialogResult.OK)
        {
            ApplySettings();
        }
    }

    private void menuEditFlip_Click(object sender, EventArgs e)
    {
        foreach (Track t in trackManager.SelectedTracks)
        {
            t.Flip();
            t.PanDrawSamples.Invalidate();
        }
    }

    private void menuEditReverse_Click(object sender, EventArgs e)
    {
        foreach (Track t in trackManager.SelectedTracks)
        {
            t.Reverse();
            t.PanDrawSamples.Invalidate();
        }
    }

    private void trkVolume_Scroll(object sender, EventArgs e)
    {
        lblVolumePercent.Text = $"{trkVolume.Value}%";

        float volume = trkVolume.Value / 100.0f;
        trackManager.SetSelectedVolume(volume);
    }

    private void menuEditEliminateSilence_Click(object sender, EventArgs e)
    {
        if (trackManager.SelectedTrack is not null)
        {
            FrmEditEliminateSilence frm = new();
            frm.SetSound(trackManager.SelectedTrack);
            frm.SelectedTracks = trackManager.SelectedTracks.ToList();
            frm.ShowDialog();
        }
    }

    private void menuGenRecord_Click(object sender, EventArgs e)
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        try
        {
            if (WaveIn.DeviceCount == 0)
            {
                throw new InvalidOperationException("No audio input devices are available.");
            }

            int deviceNumber = Settings.AudioInputDeviceNumber;
            if (deviceNumber < 0 || deviceNumber >= WaveIn.DeviceCount)
            {
                deviceNumber = 0;
                Settings.AudioInputDeviceNumber = deviceNumber;
            }

            recordingTrack = new Track(Array.Empty<float>());
            CreateTrackPanel(recordingTrack);
            trackManager.AddTrack(recordingTrack);
            SetDefaultZoom();
            UpdateSizes();

            lock (recordedSamplesLock)
            {
                recordedSamples.Clear();
            }
            renderedRecordingSampleCount = 0;

            waveSource = new WaveIn
            {
                DeviceNumber = deviceNumber,
                WaveFormat = new WaveFormat(Track.INTERNAL_SAMPLE_RATE, 16, Track.INTERNAL_CHANNELS)
            };
            waveSource.DataAvailable += WaveSource_DataAvailable;
            waveSource.RecordingStopped += WaveSource_RecordingStopped;
            waveSource.StartRecording();

            isRecording = true;
            bttnRecord.BackgroundImage = Resources.stop;
            tmrUpdate.Start();
        }
        catch (Exception ex)
        {
            isRecording = false;
            bttnRecord.BackgroundImage = Resources.record;
            waveSource?.Dispose();
            waveSource = null;
            RemoveRecordingTrack();
            MessageBox.Show($"Failed to start recording: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        UpdateLiveRecording(force: true);
        waveSource?.StopRecording();
        bttnRecord.BackgroundImage = Resources.record;

        if (trackManager.State != PlaybackState.Playing)
        {
            tmrUpdate.Stop();
        }
    }

    private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
    {
        WaveFormat format = ((WaveIn)sender).WaveFormat;
        int bytesPerSample = format.BitsPerSample / 8;
        int channels = format.Channels;
        int frameSize = bytesPerSample * channels;
        int frameCount = e.BytesRecorded / frameSize;

        lock (recordedSamplesLock)
        {
            for (int i = 0; i < frameCount; i++)
            {
                float sum = 0;
                for (int c = 0; c < channels; c++)
                {
                    int offset = (i * frameSize) + (c * bytesPerSample);
                    sum += BitConverter.ToInt16(e.Buffer, offset) / 32768f;
                }
                recordedSamples.Add(sum / channels);
            }
        }
    }

    private void WaveSource_RecordingStopped(object sender, StoppedEventArgs e)
    {
        waveSource?.Dispose();
        waveSource = null;
    }

    private void UpdateLiveRecording(bool force = false)
    {
        if (recordingTrack is null) return;

        float[] samples;
        lock (recordedSamplesLock)
        {
            if (!force && recordedSamples.Count == renderedRecordingSampleCount) return;
            samples = recordedSamples.ToArray();
        }

        renderedRecordingSampleCount = samples.Length;
        recordingTrack.ReplaceSamples(samples);
        recordingTrack.PanDrawSamples?.Invalidate();
        UpdateSizes();
    }

    private void RemoveRecordingTrack()
    {
        if (recordingTrack is null) return;

        if (recordingTrack.PanDrawSamples is not null)
        {
            panTracks.Controls.Remove(recordingTrack.PanDrawSamples.Parent);
            recordingTrack.PanDrawSamples.Parent?.Dispose();
        }
        recordingTrack.Dispose();
        trackManager.RemTrack(recordingTrack);
        recordingTrack = null;
        UpdateSizes();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        StopRecording();
        base.OnFormClosing(e);
    }

    private void menuGenNoise_Click(object sender, EventArgs e)
    {
        const int NUM_SAMPLES = 100_000;
        float[] samples = new float[NUM_SAMPLES];
        Random rand = new();
        for (int i = 0; i < NUM_SAMPLES; i++)
        {
            samples[i] = rand.NextSingle() * 2.0f - 1.0f;
        }
        Track newTrack = new(samples);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
        SetDefaultZoom();
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();
    }
    private void menuGenGradientNoise_Click(object sender, EventArgs e)
    {
        const int NUM_SAMPLES = 250_000;
        float[] samples = new float[NUM_SAMPLES];
        Random rand = new();
        float cur = 0;
        float inc = 0.005f;
        int dir = +1;
        for (int i = 0; i < NUM_SAMPLES; i++)
        {
            cur += (inc * dir);
            if (rand.NextSingle() < 0.5f)
            {
                dir *= -1;
            }
        }
        Track newTrack = new(samples);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
        SetDefaultZoom();
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();
    }

    private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e)
    {
        UpdateLiveRecording();

        if (trackManager.Tracks.Count > 0)
        {
            // playback ran off the end: park the playhead at the end and stop polling
            if (!isRecording && tmrUpdate.Enabled && trackManager.State == NAudio.Wave.PlaybackState.Stopped)
            {
                trackManager.OnPlaybackEnded();
                tmrUpdate.Stop();
            }
            bttnPlay.BackgroundImage = trackManager.State == NAudio.Wave.PlaybackState.Playing ? Resources.pause : Resources.play;
            bttnPlay.Visible = true;
            lblCurrTime.Visible = true;
            if (!isScrubbing)
            {
                RefreshPlayheadVisuals();
            }
            lblCurrTime.Update();
        }
        else
        {
            bttnPlay.BackgroundImage = Resources.pause;
            bttnPlay.Visible = true;
            tmrUpdate.Stop();
            lblPlayhead.Left = 0; // Reset playhead to start
            lblCurrTime.Visible = true;
            lblCurrTime.Text = "00:00.00/00:00.00";
            lblCurrTime.Update();
        }
    }

    // playhead slider
    private void RefreshPlayheadVisuals()
    {
        int sample = trackManager.State == PlaybackState.Playing
            ? trackManager.PlayheadSample
            : isRecording ? recordingTrack?.AdjustedSampleCount ?? trackManager.PlayheadSample : trackManager.PlayheadSample;
        ShowPlayheadAt(sample);
    }

    private void ShowPlayheadAt(int sample)
    {
        int total = trackManager.TotalSamples;
        sample = Math.Clamp(sample, 0, total);
        int scale = Math.Max(1, trackManager.DrawTimescale);
        int x = sample / scale;

        if (x != playlineX)
        {
            int oldX = playlineX;
            playlineX = x;
            InvalidatePlayline(oldX); // erase the old line
            InvalidatePlayline(x);    // draw the new one
        }

        sldPlayhead.MaxContentX = total / scale;
        sldPlayhead.OriginX = GetContentOriginInSlider();
        sldPlayhead.ContentX = x;
        sldPlayhead.Update();

        TimeSpan cur = TimeSpan.FromSeconds((double)sample / Track.INTERNAL_SAMPLE_RATE);
        TimeSpan tot = trackManager.TotalTime;
        lblCurrTime.Text = $"{cur:mm\\:ss\\.fff} / {tot:mm\\:ss\\.fff}";
        FollowPlayhead(x);
    }

    private void FollowPlayhead(int playheadX)
    {
        if (!isRecording && trackManager.State != PlaybackState.Playing)
        {
            return;
        }

        int viewportWidth = panTracks.ClientSize.Width;
        if (viewportWidth <= 0)
        {
            return;
        }

        int scrollX = -panTracks.AutoScrollPosition.X;
        int margin = Math.Max(40, viewportWidth / 4);
        int visibleLeft = scrollX + margin;
        int visibleRight = scrollX + viewportWidth - margin;

        if (playheadX >= visibleLeft && playheadX <= visibleRight)
        {
            return;
        }

        int maxScroll = Math.Max(0,
            panTracks.HorizontalScroll.Maximum - panTracks.HorizontalScroll.LargeChange + 1);
        int targetScroll = Math.Clamp(playheadX - viewportWidth / 2, 0, maxScroll);
        panTracks.AutoScrollPosition = new Point(targetScroll, -panTracks.AutoScrollPosition.Y);
    }

    // --- playline painting helpers ---
    private int PlaylineWidth => Math.Max(1, lblPlayhead.Width);

    private Control GetReferenceSurface() =>
        trackManager.Tracks.Select(t => t.PanDrawSamples).FirstOrDefault(p => p is { IsDisposed: false, IsHandleCreated: true });

    // x of the playline's left edge in 'surface' coordinates (handles scroll offset and borders)
    private int PlaylineLeftIn(Control surface, int contentX)
    {
        Control reference = GetReferenceSurface();
        if (reference is null) return contentX;
        int screenX = reference.PointToScreen(new Point(contentX - PlaylineWidth / 2, 0)).X;
        return surface.PointToClient(new Point(screenX, 0)).X;
    }

    private int GetContentOriginInSlider()
    {
        Control reference = GetReferenceSurface();
        if (reference is null) return 0;
        return sldPlayhead.PointToClient(reference.PointToScreen(Point.Empty)).X;
    }

    private void DrawPlayline(Graphics g, Control surface)
    {
        if (trackManager is null || trackManager.Tracks.Count == 0) return;
        using SolidBrush brush = new(Settings.Color.Playline);
        g.FillRectangle(brush, PlaylineLeftIn(surface, playlineX), 0, PlaylineWidth, surface.ClientSize.Height);
    }

    // repaint only a thin vertical strip around the line on every surface it crosses
    private void InvalidatePlayline(int contentX)
    {
        List<Control> surfaces = new() { panTracks };
        foreach (Track t in trackManager.Tracks)
        {
            Control p = t.PanDrawSamples;
            if (p is null || p.IsDisposed) continue;
            if (p.Parent is not null)
            {
                surfaces.Add(p.Parent);
                // transparent children (track name, buttons) need their own invalidation
                foreach (Control child in p.Parent.Controls)
                {
                    if (child != p) surfaces.Add(child);
                }
            }
            surfaces.Add(p);
        }

        foreach (Control s in surfaces)
        {
            Rectangle r = new(PlaylineLeftIn(s, contentX) - 1, 0, PlaylineWidth + 2, s.ClientSize.Height);
            s.Invalidate(r);
        }
        foreach (Control s in surfaces)
        {
            s.Update();
        }
    }

    private void SldPlayhead_Scrubbing(object sender, int contentX)
    {
        // just preview while dragging; the audio jumps when the mouse is released
        isScrubbing = true;
        ShowPlayheadAt(contentX * Math.Max(1, trackManager.DrawTimescale));
    }

    private void SldPlayhead_ScrubCompleted(object sender, int contentX)
    {
        trackManager.SeekToSample(contentX * Math.Max(1, trackManager.DrawTimescale));
        isScrubbing = false;
        RefreshPlayheadVisuals();
    }

    private void trkDrawTimescale_Scroll(object sender, EventArgs e)
    {
        trackManager.DrawTimescale = GetTimescaleFromSlider();
        UpdateSizes();
        trackManager.RedrawAll();
    }

    // Helper methods
    private Panel CreateTrackPanel(Track track)
    {
        Panel panTrackContainer = new()
        {
            BackColor = Settings.Color.TrackBg_Container,
            Width = panTracks.Width,
            Left = 0,
            Top = 0,
            BorderStyle = BorderStyle.None // border-less so the playline is continuous across tracks
        };
        Button bttnMute = new()
        {
            BackColor = Color.Transparent,
            BackgroundImage = Resources.volume,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.Transparent, MouseOverBackColor = Color.Transparent },
            Size = new Size(34, 35),
            Location = new Point(43, 6),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
        };
        Button bttnDelete = new()
        {
            BackColor = Color.Transparent,
            BackgroundImage = Resources.close,
            BackgroundImageLayout = ImageLayout.Stretch,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.Transparent, MouseOverBackColor = Color.Transparent },
            Size = new Size(34, 35),
            Location = new Point(3, 6),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
        };

        string fileName = track.GetFileName();
        Label lblTrackName = new()
        {
            Text = string.IsNullOrWhiteSpace(fileName) ? $"Track {trackManager.TrackCounter}" : fileName,
            AutoSize = true,
            Location = new Point(100, 6),
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Settings.Color.WinFg,
            BackColor = Color.Transparent // lets the playline show behind the track name
        };
        DoubleBufferedPanel panTrack = new()
        {
            BackColor = Settings.Color.TrackBg_Unselected,
            Width = panTracks.Width,
            Left = 0,
            Location = new Point(0, 45),
            BorderStyle = BorderStyle.None
        };

        // add inner elements to the track container panel
        panTrackContainer.Controls.Add(bttnMute);
        panTrackContainer.Controls.Add(panTrack);
        panTrackContainer.Controls.Add(bttnDelete);
        panTrackContainer.Controls.Add(lblTrackName);

        int dragStartX = 0;
        bool isDragging = false;

        bttnMute.Click += (sender, e) =>
        {
            track.ToggleDisabled();
            bttnMute.BackgroundImage = track.IsDisabled ? Resources.mute : Resources.volume;
            track.PanDrawSamples.Invalidate();
        };
        panTrack.DoubleClick += (sender, e) =>
        {
            track.ToggleDisabled();
            bttnMute.BackgroundImage = track.IsDisabled ? Resources.mute : Resources.volume;
            track.PanDrawSamples.Invalidate();
        };

        bttnDelete.Click += (sender, e) =>
        {
            IEnumerable<Track> tracksToDelete = trackManager.SelectedTracks.Contains(track)
                ? trackManager.SelectedTracks
                : new[] { track };
            DeleteTracks(tracksToDelete);
        };

        Action<MouseEventArgs, bool> handleSelectionDrag = (mouseArgs, isMouseUp) =>
        {
            if (mouseArgs.Button != MouseButtons.Left)
            {
                return;
            }

            bool isCtrlHeld = ModifierKeys.HasFlag(Keys.Control);

            if (!isMouseUp)
            {
                if (isCtrlHeld)
                {
                    trackManager.SelectTrack(track, toggle: true);
                }
                else if (!trackManager.SelectedTracks.Contains(track))
                {
                    trackManager.SelectTrack(track, toggle: false);
                }

                dragStartX = mouseArgs.X;
                isDragging = true;
                return;
            }

            if (isDragging)
            {
                isDragging = false;

                if (Math.Abs(mouseArgs.X - dragStartX) < 3)
                {
                    trackManager.ClearSelection();
                    if (!isCtrlHeld)
                    {
                        trackManager.SelectTrack(track, toggle: false);
                    }
                }
                else
                {
                    trackManager.SetSelection(dragStartX, mouseArgs.X);
                }
            }
        };

        panTrack.MouseDown += (sender, e) => handleSelectionDrag(e, false);
        panTrack.MouseMove += (sender, e) =>
        {
            if (isDragging)
            {
                trackManager.SetSelection(dragStartX, e.X);
            }
        };
        panTrack.MouseUp += (sender, e) => handleSelectionDrag(e, true);
        panTrackContainer.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                handleSelectionDrag(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta), false);
            }
        };
        panTrackContainer.MouseMove += (sender, e) =>
        {
            if (isDragging)
            {
                trackManager.SetSelection(dragStartX, e.X);
            }
        };
        panTrackContainer.MouseUp += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                handleSelectionDrag(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta), true);
            }
        };

        // Only render the selection box if THIS track is currently selected
        panTrack.Paint += (sender, e) =>
        {
            bool isSelected = trackManager.SelectedTracks.Contains(track);
            int? selStart = isSelected ? trackManager.SelectionStartX : null;
            int? selEnd = isSelected ? trackManager.SelectionEndX : null;

            track.DrawSamples(e.Graphics, trackManager.DrawTimescale, selStart, selEnd, e.ClipRectangle);
            DrawPlayline(e.Graphics, panTrack);
        };

        panTrackContainer.Paint += (sender, e) => DrawPlayline(e.Graphics, panTrackContainer);

        panTracks.Controls.Add(panTrackContainer);
        track.LinkPanels(panTrack, panTracks);
        trackManager.SelectTrack(track);

        return panTrackContainer;
    }

    private void DeleteTracks(IEnumerable<Track> tracks)
    {
        List<Track> tracksToDelete = tracks.Distinct().ToList();
        if (tracksToDelete.Count == 0)
        {
            return;
        }

        string message = tracksToDelete.Count == 1
            ? "Are you sure you want to delete this track? This is NOT undoable!"
            : "Are you sure you want to delete the selected tracks? This is NOT undoable!";
        if (MessageBox.Show(message, "Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        {
            return;
        }

        foreach (Track track in tracksToDelete)
        {
            Control trackContainer = track.PanDrawSamples?.Parent;
            if (trackContainer is not null)
            {
                panTracks.Controls.Remove(trackContainer);
                trackContainer.Dispose();
            }

            track.Dispose();
            trackManager.RemTrack(track);
        }

        UpdateSizes();
    }

    public void RefreshTrackLayout()
    {
        UpdateSizes();
    }

    private void UpdateSizes()
    {
        int availableWidth = panApp.ClientSize.Width;

        // align top action panel
        panTopActions.Left = 0;
        panTopActions.Top = menu.Bottom;
        panTopActions.Width = availableWidth;

        int volumeWidth = Math.Clamp(availableWidth / 8, 160, 220);
        trkVolume.Size = new Size(volumeWidth, 40);
        lblVolumePercent.Left = availableWidth - 15 - lblVolumePercent.Width;
        trkVolume.Left = lblVolumePercent.Left - 8 - trkVolume.Width;
        picVolume.Left = trkVolume.Left - 8 - picVolume.Width;
        int topControlTop = (panTopActions.ClientSize.Height - trkVolume.Height) / 2;
        trkVolume.Top = topControlTop;
        picVolume.Top = (panTopActions.ClientSize.Height - picVolume.Height) / 2;
        lblVolumePercent.Top = (panTopActions.ClientSize.Height - lblVolumePercent.Height) / 2;

        // align bottom action panel
        panBottomActions.Left = 0;
        panBottomActions.Top = panApp.ClientSize.Height - panBottomActions.Height;
        panBottomActions.Width = availableWidth;

        // align middle area holding tracks
        sldPlayhead.Left = 0;
        sldPlayhead.Top = panTopActions.Bottom;
        sldPlayhead.Width = availableWidth;
        panTracks.Left = 0;
        panTracks.Top = sldPlayhead.Bottom;
        panTracks.Width = availableWidth;
        panTracks.Height = Math.Max(50, panBottomActions.Top - panTracks.Top);

        int contentWidth = GetTrackContentWidth();

        // align playhead label
        lblPlayhead.Height = panTracks.ClientSize.Height;
        lblPlayhead.BringToFront();

        // align timer with center of screen
        lblCurrTime.AutoSize = true;
        lblCurrTime.Left = (panBottomActions.ClientSize.Width - lblCurrTime.Width) / 2;
        lblCurrTime.Top = (panBottomActions.ClientSize.Height - lblCurrTime.Height) / 2;

        // keep the timescale controls on the right while scaling their widths with the window
        int timescaleWidth = Math.Clamp(availableWidth / 6, 220, 300);
        trkDrawTimescale.Width = timescaleWidth;
        trkDrawTimescale.Left = panBottomActions.ClientSize.Width - timescaleWidth - 15;
        picSoundWave.Left = trkDrawTimescale.Left - picSoundWave.Width - 8;
        picSoundWave.Top = (panBottomActions.ClientSize.Height - picSoundWave.Height) / 2;

        trkDrawTimescale.Top = (panBottomActions.ClientSize.Height - trkDrawTimescale.Height) / 2;

        RefreshPlayheadVisuals();

        // adjust track sizing
        if (trackManager.Tracks.Count == 0) return;

        int trackCount = trackManager.Tracks.Count;
        int containerHeight = 256;
        int top = 0;

        // children of an AutoScroll panel are positioned in scrolled coordinates, so keep the
        // current scroll offset when placing them (resetting to 0 made the scroll range grow)
        Point scrollOffset = panTracks.AutoScrollPosition; // x/y are <= 0

        foreach (Control ctrl in panTracks.Controls)
        {
            if (ctrl is Panel panTrackContainer && ctrl != lblPlayhead)
            {
                panTrackContainer.Left = scrollOffset.X;
                panTrackContainer.Width = contentWidth;
                panTrackContainer.Height = containerHeight;
                panTrackContainer.Top = scrollOffset.Y + top;
                top += containerHeight + 4;

                foreach (Control child in panTrackContainer.Controls)
                {
                    if (child is DoubleBufferedPanel panTrack)
                    {
                        panTrack.Left = 0;
                        panTrack.Top = 45;
                        panTrack.Width = contentWidth;
                        panTrack.Height = Math.Max(10, panTrackContainer.Height - 45);
                        panTrack.Invalidate();
                    }
                }
            }
        }
        lblPlayhead.Height = Math.Max(panTracks.ClientSize.Height, top);
        lblPlayhead.BringToFront();
        RefreshPlayheadVisuals();
    }

    private int GetTrackContentWidth()
    {
        int viewportWidth = Math.Max(1, panTracks.ClientSize.Width);
        int contentWidth = viewportWidth;

        foreach (Track track in trackManager.Tracks)
        {
            int waveformWidth = (int)Math.Ceiling((double)track.SampleCount / Math.Max(1, trackManager.DrawTimescale));
            contentWidth = Math.Max(contentWidth, waveformWidth + track.DrawOffset + 1);
        }

        return contentWidth;
    }

    private void panTracks_Paint(object sender, PaintEventArgs e)
    {

    }

    private void FrmMain_Load(object sender, EventArgs e)
    {

    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void bttnGenerate_Click(object sender, EventArgs e)
    {
        using FrmGenerate frmGenerate = new();
        frmGenerate.ShowDialog(this);
    }
}

public class DoubleBufferedPanel : Panel
{
    public DoubleBufferedPanel()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        UpdateStyles();
    }
}
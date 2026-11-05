using MediocreSoundDesigner.Properties;
using System.Drawing.Printing;
using System.Text.Json;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form
{
    // fields
    private TrackManager trackManager;

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
        panTracks.AutoScroll = false;

        // Enable double-buffering on panTracks container to prevent child clipping flicker
        typeof(Panel).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.SetProperty |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic,
            null, panTracks, new object[] { true });

        trackManager = new(panTracks);
        bttnPlay.BackgroundImage = Resources.play;
        trackManager.DrawTimescale = trkDrawTimescale.Value;
        UpdateSizes();
        ApplySettings();
    }
    private void ApplySettings()
    {
        bttnPlay.BackgroundImage = Resources.play;

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
                if (MessageBox.Show("This will delete the selected track(s). This is NOT undoable!", "Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    var toDelete = trackManager.SelectedTracks.ToList();
                    foreach (Track t in toDelete)
                    {
                        panTracks.Controls.Remove(t.PanDrawSamples);
                        t.PanDrawSamples.Dispose();
                        t.Dispose();
                        trackManager.RemTrack(t);
                    }
                    UpdateSizes();
                    return true;
                }
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
            tmrUpdate.Stop();
        }
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
                Track newTrack = new(file);
                CreateTrackPanel(newTrack);
                trackManager.AddTrack(newTrack);
                FitToWindow();
                UpdateSizes();
                newTrack.PanDrawSamples.Invalidate();
            }
        }
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

    private void menuEditVolume_Click(object sender, EventArgs e)
    {
        if (trackManager.SelectedTrack is not null)
        {
            FrmEditVolume frm = new();
            frm.SetSound(trackManager.SelectedTrack);
            frm.SelectedTracks = trackManager.SelectedTracks.ToList();
            frm.ShowDialog();
        }
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
        Track newTrack = new(new float[0]);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
        FitToWindow();
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();

        FrmGenRecord frm = new();
        frm.SetSound(newTrack);
        if (frm.ShowDialog() == DialogResult.Cancel)
        {
            panTracks.Controls.Remove(newTrack.PanDrawSamples);
            newTrack.PanDrawSamples.Dispose();
            newTrack.Dispose();
            trackManager.RemTrack(newTrack);
            UpdateSizes();
        }
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
        FitToWindow();
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
        FitToWindow();
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();
    }

    private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e)
    {
        if (trackManager.Tracks.Count > 0)
        {
            bttnPlay.BackgroundImage = trackManager.State == NAudio.Wave.PlaybackState.Playing ? Resources.pause : Resources.play;
            bttnPlay.Visible = true;
            lblPlayhead.Left = trackManager.GetPlayheadPos();
            lblPlayhead.Visible = true;
            lblCurrTime.Visible = true;
            lblCurrTime.Text = $"{trackManager.CurrentTime:mm\\:ss\\.fff} / {trackManager.TotalTime:mm\\:ss\\.fff}";
            lblCurrTime.Update();
        }
        else
        {
            bttnPlay.BackgroundImage = Resources.pause;
            bttnPlay.Visible = true;
            tmrUpdate.Stop();
            lblPlayhead.Left = 0; // Reset playhead to start
            lblCurrTime.Update();
        }
    }

    private void trkDrawTimescale_Scroll(object sender, EventArgs e)
    {
        trackManager.DrawTimescale = GetTimescaleFromSlider();
        trackManager.RedrawAll();
    }

    // Helper methods
    private Panel CreateTrackPanel(Track track)
    {
        Panel panTrackContainer = new()
        {
            BackColor = Settings.Color.TrackBg_Unselected,
            Width = panTracks.Width,
            Left = 0,
            Top = 0,
            BorderStyle = BorderStyle.FixedSingle
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
            ForeColor = Color.Black
        };
        DoubleBufferedPanel panTrack = new()
        {
            BackColor = Settings.Color.TrackBg_Unselected,
            Width = panTracks.Width,
            Left = 0,
            Location = new Point(0, 45),
            BorderStyle = BorderStyle.FixedSingle
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

        bttnDelete.Click += (sender, e) =>
        {
            if (MessageBox.Show("Are you sure you want to delete this track? This is NOT undoable!", "Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                panTracks.Controls.Remove(panTrackContainer);
                panTrackContainer.Dispose();
                track.PanDrawSamples.Dispose();
                track.Dispose();
                trackManager.RemTrack(track);
                UpdateSizes();
            }
        };

        panTrackContainer.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                bool isCtrlHeld = ModifierKeys.HasFlag(Keys.Control);

                if (isCtrlHeld)
                {
                    // Toggle individual track selection
                    trackManager.SelectTrack(track, toggle: true);
                }
                else
                {
                    // If clicking an unselected track, select only that track
                    // If clicking an ALREADY selected track, keep the multi-selection intact for dragging
                    if (!trackManager.SelectedTracks.Contains(track))
                    {
                        trackManager.SelectTrack(track, toggle: false);
                    }
                }

                dragStartX = e.X;
                isDragging = true;
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
            if (e.Button == MouseButtons.Left && isDragging)
            {
                isDragging = false;
                bool isCtrlHeld = ModifierKeys.HasFlag(Keys.Control);

                // Single click without dragging
                if (Math.Abs(e.X - dragStartX) < 3)
                {
                    trackManager.ClearSelection();
                    // If Ctrl wasn't held, collapse multi-selection down to this single clicked track
                    if (!isCtrlHeld)
                    {
                        trackManager.SelectTrack(track, toggle: false);
                    }
                }
                else
                {
                    trackManager.SetSelection(dragStartX, e.X);
                }
            }
        };

        panTrackContainer.DoubleClick += (sender, e) =>
        {
            track.ToggleDisabled();
            track.PanDrawSamples.Invalidate();
        };

        // Only render the selection box if THIS track is currently selected
        panTrack.Paint += (sender, e) =>
        {
            bool isSelected = trackManager.SelectedTracks.Contains(track);
            int? selStart = isSelected ? trackManager.SelectionStartX : null;
            int? selEnd = isSelected ? trackManager.SelectionEndX : null;

            track.DrawSamples(e.Graphics, trackManager.DrawTimescale, selStart, selEnd);
        };

        panTracks.Controls.Add(panTrackContainer);
        track.LinkPanels(panTrack, panTracks);
        trackManager.SelectTrack(track);

        return panTrackContainer;
    }

    private void UpdateSizes()
    {
        int availableWidth = panApp.ClientSize.Width;

        // align top action panel
        panTopActions.Left = 0;
        panTopActions.Top = menu.Bottom;
        panTopActions.Width = availableWidth;

        // align bottom action panel
        panBottomActions.Left = 0;
        panBottomActions.Top = panApp.ClientSize.Height - panBottomActions.Height;
        panBottomActions.Width = availableWidth;

        // align middle area holding tracks
        panTracks.Left = 0;
        panTracks.Top = panTopActions.Bottom;
        panTracks.Width = availableWidth;
        panTracks.Height = Math.Max(50, panBottomActions.Top - panTracks.Top);

        // align playhead label
        lblPlayhead.Left = 0;
        lblPlayhead.Height = panTracks.ClientSize.Height;
        lblPlayhead.BringToFront();

        // align timer with center of screen
        lblCurrTime.AutoSize = true;
        lblCurrTime.Left = (panBottomActions.ClientSize.Width - lblCurrTime.Width) / 2;
        lblCurrTime.Top = (panBottomActions.ClientSize.Height - lblCurrTime.Height) / 2;

        // align timescale trackbar with icon
        picSoundWave.Left = lblCurrTime.Right + 350;
        picSoundWave.Top = (panBottomActions.ClientSize.Height - picSoundWave.Height) / 2;

        // 4. Position Timescale Slider directly to the right of the Icon
        trkDrawTimescale.Left = picSoundWave.Right + 8;
        trkDrawTimescale.Top = (panBottomActions.ClientSize.Height - trkDrawTimescale.Height) / 2;
        trkDrawTimescale.Width = (panBottomActions.ClientSize.Width - 15) - trkDrawTimescale.Left;

        // adjust track sizing
        if (trackManager.Tracks.Count == 0) return;

        int trackCount = trackManager.Tracks.Count;
        int containerHeight = 100;
        int top = 0;

        foreach (Control ctrl in panTracks.Controls)
        {
            if (ctrl is Panel panTrackContainer && ctrl != lblPlayhead)
            {
                panTrackContainer.Left = 0;
                panTrackContainer.Width = panTracks.Width;
                panTrackContainer.Height = containerHeight;
                panTrackContainer.Top = top;
                top += containerHeight + 4;

                foreach (Control child in panTrackContainer.Controls)
                {
                    if (child is DoubleBufferedPanel panTrack)
                    {
                        panTrack.Left = 0;
                        panTrack.Top = 45;
                        panTrack.Width = panTrackContainer.Width;
                        panTrack.Height = Math.Max(10, panTrackContainer.Height - 45);
                        panTrack.Invalidate();
                    }
                }
            }
        }
        lblPlayhead.Left = 0;
        lblPlayhead.Height = Math.Max(panTracks.ClientSize.Height, top);
        lblPlayhead.BringToFront();
    }

    private void panTracks_Paint(object sender, PaintEventArgs e)
    {

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

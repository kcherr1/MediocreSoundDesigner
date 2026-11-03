using MediocreSoundDesigner.Properties;
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
        BackColor = Settings.Color.WinBg;
        panTracks.BackColor = Settings.Color.TrackBg_Unselected;
        lblPlayhead.BackColor = Settings.Color.Playline;
        trackManager.ApplySettings();
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
        }
        else
        {
            trackManager.PauseAll();
            bttnPlay.BackgroundImage = Resources.play;
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
                UpdateSizes();
                newTrack.PanDrawSamples.Invalidate();
            }
        }
    }
    private void menuFileSave_Click(object sender, EventArgs e)
    {
        sfdSaveFile.DefaultExt = "wav";
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
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();

        FrmGenRecord frm = new();
        frm.SetSound(trackManager.SelectedTrack);
        if (frm.ShowDialog() == DialogResult.Cancel)
        {
            trackManager.RemTrack(newTrack);
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
            if (cur > 0.9f)
            {
                dir = -1;
            }
            if (cur < -0.9f)
            {
                dir = +1;
            }
            samples[i] = cur;
        }
        Track newTrack = new(samples);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
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
        }
        else
        {
            bttnPlay.BackgroundImage = null;
            bttnPlay.Visible = false;
            lblPlayhead.Visible = false;
        }
    }

    private void trkDrawTimescale_Scroll(object sender, EventArgs e)
    {
        trackManager.DrawTimescale = trkDrawTimescale.Maximum - trkDrawTimescale.Value;
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
        DoubleBufferedPanel panTrack = new()
        {
            BackColor = Color.LightGray,
            Width = panTracks.Width,
            Left = 0,
            Location = new Point(0, 45),
            BorderStyle = BorderStyle.FixedSingle
        };

        // add inner elements to the track container panel
        panTrackContainer.Controls.Add(bttnMute);
        panTrackContainer.Controls.Add(panTrack);
        panTrackContainer.Controls.Add(bttnDelete);

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
    const int PADDING = 15;

    panTracks.Left = PADDING;
    panTracks.Width = ClientSize.Width - (PADDING * 2);

    if (trackManager.Tracks.Count == 0) return;

    int trackCount = trackManager.Tracks.Count;
    int containerHeight = Math.Max(100, (panTracks.Height / trackCount) - 4);
    int top = 0;

    foreach (Control ctrl in panTracks.Controls)
    {
        // track outer panel (holding buttons and track panel)
        if (ctrl is Panel panTrackContainer && ctrl != lblPlayhead)
        {
            panTrackContainer.Left = 0;
            panTrackContainer.Width = panTracks.Width;
            panTrackContainer.Height = containerHeight;
            panTrackContainer.Top = top;
            top += containerHeight + 4;

            // handle inner panel (track)
            foreach (Control child in panTrackContainer.Controls)
            {
                if (child is DoubleBufferedPanel panTrack)
                {
                    panTrack.Left = 0;
                    panTrack.Top = 45; // Positioned below bttnMute and bttnDelete
                    panTrack.Width = panTrackContainer.Width;
                    panTrack.Height = Math.Max(10, panTrackContainer.Height - 45);
                    panTrack.Invalidate();
                }
            }
        }
    }

    trkDrawTimescale.Width = panTracks.Width;
    trkDrawTimescale.Left = PADDING;
    lblPlayhead.Left = 0;
    lblPlayhead.Height = panTracks.Height;
    lblPlayhead.BringToFront();
}

    private void trackDrawTimeScale_Scroll(object sender, EventArgs e)
    {

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void panTracks_Paint(object sender, PaintEventArgs e)
    {

    }

    private void pnlWaveform_Paint(object sender, PaintEventArgs e)
    {

    }

    private void pnlTrackContainer_Paint(object sender, PaintEventArgs e)
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

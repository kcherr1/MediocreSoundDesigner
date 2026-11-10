using MediocreSoundDesigner.Properties;
using System.Drawing.Drawing2D;

namespace MediocreSoundDesigner;

public partial class FrmMain : Form
{
  // fields
  private TrackManager trackManager;
  private Panel panTimeRuler;
  private string currentProjectPath;
  private ToolTip topButtonTips;
  private ImageList smallToolbarIcons;
  private Track playbackTrack;
  private int selectionStart;
  private int selectionEnd;
  private int firstVisiblePixel;
  private int playheadPixel;
  private int lastPlayheadScreenX = -1;
  private bool isZooming;
  private System.Windows.Forms.Timer zoomSettleTimer;
  private HScrollBar hsbTimeline;


  // Use the existing generator heading.
  private TextBox lbInstPanel => lbGeneratorPanel;

  private sealed class DrawingPanel : Panel
  {
    public DrawingPanel()
    {
      DoubleBuffered = true;
      ResizeRedraw = true;
    }
  }

  protected override CreateParams CreateParams
  {
    get
    {
      CreateParams createParams = base.CreateParams;
      createParams.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
      return createParams;
    }
  }

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
    trackManager = new(panTracks);
    zoomSettleTimer = new System.Windows.Forms.Timer(components) { Interval = 180 };
    zoomSettleTimer.Tick += (sender, e) =>
    {
      zoomSettleTimer.Stop();
      isZooming = false;
      trackManager.RedrawAll();
      UpdatePlaybackView();
    };

    ofdLoadFile.InitialDirectory = Path.Combine(AppContext.BaseDirectory, "Resources");
    ofdLoadFile.Filter = "Audio files (*.wav;*.mp3)|*.wav;*.mp3|All files (*.*)|*.*";
    ofdLoadFile.FileName = "";
    smallToolbarIcons = new ImageList(components)
    {
      ColorDepth = ColorDepth.Depth32Bit,
      ImageSize = new Size(30, 30)
    };
    smallToolbarIcons.Images.Add(GetButtonIcon(buttonImport));
    smallToolbarIcons.Images.Add(GetButtonIcon(buttonDeleteSelection));

    trkDrawTimescale.Minimum = 1;
    trkDrawTimescale.Maximum = 1000;
    trkDrawTimescale.Value = 500;
    trkDrawTimescale.TickFrequency = 100;
    trackManager.DrawTimescale = trkDrawTimescale.Value;
    trkDrawTimescale.Scroll -= trkDrawTimescale_Scroll;
    trkDrawTimescale.Scroll += trkDrawTimescale_Scroll;

    panTracks.AutoScroll = false;
    lblPlayhead.Width = 2;
    lblPlayhead.Top = 0;
    lblPlayhead.Visible = false;

    //ToolStripMenuItem menuEffects = new("Effects");
    //menuEffects.DropDownItems.Add("Fade In", null, (s, e) => ApplyEffectToTrack("FadeIn"));
    //menuEffects.DropDownItems.Add("Fade Out", null, (s, e) => ApplyEffectToTrack("FadeOut"));
    //menuEffects.DropDownItems.Add("Echo", null, (s, e) => ApplyEffectToTrack("Echo"));
    //menuEffects.DropDownItems.Add("Normalize", null, (s, e) => ApplyEffectToTrack("Normalize"));
    //menuEffects.DropDownItems.Add("Muffle", null, (s, e) => ApplyEffectToTrack("Muffle"));
    //menuEffects.DropDownItems.Add("Undo Effect", null, (s, e) => UndoEffectOnTrack());
    //menuEdit.DropDownItems.Add(menuEffects);   // shows up as Edit > Effects
    ToolStripMenuItem menuEffects = new("Voice Tools");
    menuEffects.DropDownItems.Add("Voice Only (quiet the music)", null, (s, e) => ApplyEffectToTrack("VoiceOnly"));
    menuEffects.DropDownItems.Add("Remove Voice (karaoke)", null, (s, e) => ApplyEffectToTrack("RemoveVoice"));
    menuEffects.DropDownItems.Add("Deeper Voice", null, (s, e) => ApplyEffectToTrack("Deep"));
    menuEffects.DropDownItems.Add("Higher Voice", null, (s, e) => ApplyEffectToTrack("Chipmunk"));
    menuEffects.DropDownItems.Add("Undo Effect", null, (s, e) => UndoEffectOnTrack());
    menuEffects.DropDownItems.Add("Fade In", null, (s, e) => ApplyEffectToTrack("FadeIn"));
    menuEffects.DropDownItems.Add("Fade Out", null, (s, e) => ApplyEffectToTrack("FadeOut"));
    menuEdit.DropDownItems.Add(menuEffects);    // shows up as Edit > Voice Tools

    if (MainMenuStrip is not null)
    {
      HideMenuImageMargins(MainMenuStrip.Items);
    }

    panTimeRuler = new DrawingPanel
    {
      Height = 32,
      BackColor = Color.FromArgb(10, 10, 10),
      ForeColor = Color.Yellow
    };
    panTimeRuler.Paint += DrawTimeRuler;
    Controls.Add(panTimeRuler);

    hsbTimeline = new HScrollBar
    {
      Minimum = 0,
      SmallChange = 1,
      Visible = false
    };
    hsbTimeline.Scroll += hsbTimeline_Scroll;
    Controls.Add(hsbTimeline);

    imageList1.ColorDepth = ColorDepth.Depth32Bit;
    imageList1.ImageSize = new Size(30, 30);
    imageList1.Images.Clear();

    // Instrument icons: indexes 0–3
    imageList1.Images.Add(buttonDrums.BackgroundImage);
    imageList1.Images.Add(buttonGuitar.BackgroundImage);
    imageList1.Images.Add(buttonPiano.BackgroundImage);
    imageList1.Images.Add(buttonSynth.BackgroundImage);

    // Toolbar icons: indexes 4–7
    imageList1.Images.Add(Resources.play);
    imageList1.Images.Add(Resources.pause);
    imageList1.Images.Add(buttonRecord.BackgroundImage);
    imageList1.Images.Add(buttonTrim.BackgroundImage);

    // Toolbar icons: indexes 8
    imageList1.Images.Add(Resources.reset);

    buttonDrums.BackgroundImage = null;
    buttonDrums.ImageList = imageList1;
    buttonDrums.ImageIndex = 0;
    buttonDrums.Text = "Beat";

    buttonGuitar.BackgroundImage = null;
    buttonGuitar.ImageList = imageList1;
    buttonGuitar.ImageIndex = 1;
    buttonGuitar.Text = "Pluck";

    buttonPiano.BackgroundImage = null;
    buttonPiano.ImageList = imageList1;
    buttonPiano.ImageIndex = 2;
    buttonPiano.Text = "Chord";

    buttonSynth.BackgroundImage = null;
    buttonSynth.ImageList = imageList1;
    buttonSynth.ImageIndex = 3;
    buttonSynth.Text = "Synth";

    Button[] instrumentButtons = { buttonDrums, buttonGuitar, buttonPiano, buttonSynth };

    foreach (Button button in instrumentButtons)
    {
      button.Enabled = true;
      button.AutoSize = false;
      button.Size = new Size(88, 80);
      button.UseVisualStyleBackColor = false;
      button.BackColor = Color.FromArgb(58, 58, 58);
      button.ForeColor = Color.White;
      button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
      button.FlatStyle = FlatStyle.Flat;
      button.FlatAppearance.BorderSize = 1;
      button.FlatAppearance.BorderColor = Color.FromArgb(82, 82, 82);
      button.TextImageRelation = TextImageRelation.Overlay;
      button.ImageAlign = ContentAlignment.TopCenter;
      button.TextAlign = ContentAlignment.BottomCenter;
    }

    btnPlay.BackgroundImage = null;
    btnPlay.ImageList = imageList1;
    btnPlay.ImageIndex = 4;

    buttonRecord.BackgroundImage = null;
    buttonRecord.ImageList = imageList1;
    buttonRecord.ImageIndex = 6;

    buttonTrim.BackgroundImage = null;
    buttonTrim.ImageList = imageList1;
    buttonTrim.ImageIndex = 7;

    buttonReset.BackgroundImage = null;
    buttonReset.ImageList = imageList1;
    buttonReset.ImageIndex = 8;
    buttonReset.ImageAlign = ContentAlignment.MiddleCenter;

    buttonImport.ImageList = smallToolbarIcons;
    buttonImport.ImageIndex = 0;
    buttonImport.Image = null;
    buttonImport.BackgroundImage = null;
    buttonDeleteSelection.ImageList = smallToolbarIcons;
    buttonDeleteSelection.ImageIndex = 1;
    buttonDeleteSelection.Image = null;
    buttonDeleteSelection.BackgroundImage = null;

    Button[] toolbarButtons = { btnPlay, buttonReset, buttonRecord, buttonImport, buttonTrim, buttonDeleteSelection };

    foreach (Button button in toolbarButtons)
    {
      button.AutoSize = false;
      button.Size = new Size(48, 48);
      button.Text = "";
      button.ImageAlign = ContentAlignment.MiddleCenter;
      button.UseVisualStyleBackColor = false;
      button.BackColor = Color.FromArgb(58, 58, 58);
      button.FlatStyle = FlatStyle.Flat;
      button.FlatAppearance.BorderSize = 1;
      button.FlatAppearance.BorderColor = Color.FromArgb(82, 82, 82);
      button.FlatAppearance.MouseOverBackColor = Color.FromArgb(72, 72, 72);
    }

    topButtonTips = new ToolTip
    {
      ShowAlways = true,
      InitialDelay = 500,
      AutoPopDelay = 5000,
      ReshowDelay = 100
    };
    topButtonTips.SetToolTip(btnPlay, "Play/Pause");
    topButtonTips.SetToolTip(buttonRecord, "Record");
    topButtonTips.SetToolTip(buttonImport, "Import Audio");
    topButtonTips.SetToolTip(buttonTrim, "Trim");
    topButtonTips.SetToolTip(buttonDeleteSelection, "Delete Selection");
    topButtonTips.SetToolTip(buttonReset, "Reset to the start");

    tmrUpdate.Interval = 30;
    UpdateSizes();
    ApplySettings();
  }

  private static Image GetButtonIcon(Button button)
  {
    if (button.Image is not null) return button.Image;
    if (button.BackgroundImage is not null) return button.BackgroundImage;
    if (button.ImageList is not null && button.ImageIndex >= 0 && button.ImageIndex < button.ImageList.Images.Count)
      return button.ImageList.Images[button.ImageIndex];
    throw new InvalidOperationException($"The {button.Name} button has no icon assigned.");
  }

  private void ApplySettings()
  {
    BackColor = Settings.Color.WinBg;
    panTracks.BackColor = Settings.Color.TrackBg_Unselected;
    lblPlayhead.BackColor = Settings.Color.Playline;
    lbInstPanel.BackColor = Settings.Color.WinBg;
    trackManager.ApplySettings();
  }

  private void SetTheme(bool dark)
  {
        Settings.Color.WinBg = dark ? Color.FromArgb(25, 25, 25) : Color.FromArgb(240, 240, 240);
        Settings.Color.WinFg = dark ? Color.White : Color.Black;


  }

  // GUI controls
  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {

    if (ActiveControl is not TextBoxBase)
    {
            if (keyData == Keys.D)
            {
                SetTheme(true); return true;
            }
            if (keyData == Keys.L)
            {
                SetTheme(false); return true;
            }
    }

    Track selected = GetSelectedTrack();
    if (selected is not null)
    {
      if (keyData == Keys.Left || keyData == Keys.Right)
      {
        selected.ShiftSampleOver(keyData == Keys.Left ? -5 : 5);
        selected.PanDrawSamples.Invalidate();
        return true;
      }
      if (keyData == Keys.Delete && MessageBox.Show(
        "This will delete the selected track. This is NOT undoable!",
        "Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      {
        RemoveTrack(selected);
        return true;
      }
    }
    return base.ProcessCmdKey(ref msg, keyData);
  }

  protected override void OnFormClosing(FormClosingEventArgs e)
  {
        base.OnFormClosing(e);

        if (trackManager is null || trackManager.Tracks.Count == 0)
            return;

        DialogResult choice = MessageBox.Show(this, "Do you want to save the project before closing?", "Save project", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        if (choice == DialogResult.Cancel)
        {
            e.Cancel = true;
        }

        else if (choice == DialogResult.Yes && !SaveCurrentProject(false))
        {
            e.Cancel = true;
        }
  }


  private bool SaveCurrentProject(bool saveAs)
  {
    string path = currentProjectPath;

    if (saveAs || path is null)
    {
      using SaveFileDialog dialog = new()
      {
        Filter = "Sound Designer Project (*.soundproj)|*.soundproj",
        DefaultExt = "soundproj",
        AddExtension = true,
        FileName = path is null ? "" : Path.GetFileName(path)
      };

      if (dialog.ShowDialog() != DialogResult.OK)
      {
        return false;
      }

      path = dialog.FileName;
    }

    try
    {
      trackManager.SaveProject(path);
      currentProjectPath = path;
      return true;
    }
    catch (Exception ex)
    {
      ShowOperationError("save the project", ex);
      return false;
    }
  }

  private void ShowOperationError(string operation, Exception ex)
  {
    MessageBox.Show(this, $"Could not {operation}.\n\n{ex.Message}", "Mediocre Sound Designer", MessageBoxButtons.OK, MessageBoxIcon.Error);
  }

  private bool ConfirmSaveBeforeReplacingProject()
  {
    if (trackManager.Tracks.Count == 0) return true;

    DialogResult choice = MessageBox.Show(
      this,
      "Save the current project before replacing it?",
      "Replace Project",
      MessageBoxButtons.YesNoCancel,
      MessageBoxIcon.Question);

    if (choice == DialogResult.Cancel) return false;
    if (choice == DialogResult.Yes) return SaveCurrentProject(false);

    return true;
  }

  private void FrmMain_SizeChanged(object sender, EventArgs e)
  {
    UpdateSizes();
  }
  private void btnPlay_Click(object sender, EventArgs e)
  {
    if (trackManager.Tracks.Count == 0) return;

    if (trackManager.State == NAudio.Wave.PlaybackState.Playing)
    {
      trackManager.PauseAll();
    }
    else
    {
      trackManager.PlayAll();
    }

    UpdatePlaybackView();
  }


  private void menuFileOpen_Click(object sender, EventArgs e)
  {
    ofdLoadFile.DefaultExt = "wav";
    if (ofdLoadFile.ShowDialog() == DialogResult.OK)
    {
      try
      {
        Track newTrack = new(ofdLoadFile.FileName);
        CreateTrackPanel(newTrack);
        trackManager.AddTrack(newTrack);
        SelectTrack(newTrack);
        UpdateSizes();
        newTrack.PanDrawSamples.Invalidate();
      }
      catch (Exception ex)
      {
        ShowOperationError("import the audio file", ex);
      }
    }
  }
  private void menuFileSave_Click(object sender, EventArgs e)
  {
    if (trackManager.Tracks.Count == 0 || trackManager.Tracks.All(t => t.IsDisabled))
    {
      MessageBox.Show("There are no enabled tracks to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
      return;
    }

    sfdSaveFile.Filter = "WAV Audio (*.wav)|*.wav|MP3 Audio (*.mp3)|*.mp3";
    sfdSaveFile.DefaultExt = "wav";
    sfdSaveFile.AddExtension = true;
    if (sfdSaveFile.ShowDialog() != DialogResult.OK)
    {
      return;
    }

    string ext = sfdSaveFile.FilterIndex == 2 ? ".mp3" : ".wav";
    string path = sfdSaveFile.FileName;
    string current = Path.GetExtension(path);
    if (current.Equals(".wav", StringComparison.OrdinalIgnoreCase) || current.Equals(".mp3", StringComparison.OrdinalIgnoreCase))
    {
      path = Path.ChangeExtension(path, ext);
    }
    else if (!path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
    {
      path += ext;
    }

    try
    {
      Cursor = Cursors.WaitCursor;
      trackManager.SaveToFile(path);
    }
    catch (Exception ex)
    {
      ShowOperationError("export the audio file", ex);
    }
    finally
    {
      Cursor = Cursors.Default;
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
    trackManager.SelectedTrack?.Flip();
  }
  private void menuEditReverse_Click(object sender, EventArgs e)
  {
    trackManager.SelectedTrack?.Reverse();
  }
  private void menuEditVolume_Click(object sender, EventArgs e)
  {
    if (trackManager.SelectedTrack is not null)
    {
      FrmEditVolume frm = new();
      frm.SetSound(trackManager.SelectedTrack);
      frm.ShowDialog();
    }
  }
  private void menuEditEliminateSilence_Click(object sender, EventArgs e)
  {
    if (trackManager.SelectedTrack is not null)
    {
      FrmEditEliminateSilence frm = new();
      frm.SetSound(trackManager.SelectedTrack);
      frm.ShowDialog();
    }
  }

  private int GetNextRecordingNumber()
  {
    int number = 1;
    while (trackManager.Tracks.Any(track => track.DisplayName == $"Recording {number}"))
    {
      number++;
    }
    return number;
  }

  private void menuGenRecord_Click(object sender, EventArgs e)
  {
    Track newTrack = new(new float[0]);

    using FrmGenRecord frm = new();
    frm.SetSound(newTrack);

    if (frm.ShowDialog(this) != DialogResult.OK) return;

    newTrack.SetDisplayName($"Recording {GetNextRecordingNumber()}");
    CreateTrackPanel(newTrack);
    trackManager.AddTrack(newTrack);
    SelectTrack(newTrack);
    UpdateSizes();
    newTrack.PanDrawSamples.Invalidate();
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
    SelectTrack(newTrack);
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
    SelectTrack(newTrack);
    UpdateSizes();
    newTrack.PanDrawSamples.Invalidate();
  }
  private void tmrUpdatePlaybackState_Tick(object sender, EventArgs e)
  {
    UpdatePlaybackView();
  }
  private void trkDrawTimescale_Scroll(object sender, EventArgs e)
  {
    if (trackManager is null) return;
    if (!isZooming && lastPlayheadScreenX >= 0)
    {
      lblPlayhead.Visible = false;
      InvalidatePlayheadColumn(lastPlayheadScreenX);
    }
    isZooming = true;
    zoomSettleTimer.Stop();
    zoomSettleTimer.Start();
    trackManager.DrawTimescale = Math.Max(1, trkDrawTimescale.Value);
    firstVisiblePixel = 0;
    trackManager.RedrawAll();
    UpdatePlaybackView();
  }

  private void ApplyEffectToTrack(string effect)
  {
    Track track = GetSelectedTrack();
    if (track is null) return;

    track.ApplyEffect(effect);
    AfterEdit(track);          // your existing method: resets playback and repaints
  }

  private void UndoEffectOnTrack()
  {
    Track track = GetSelectedTrack();
    if (track is null || !track.CanUndoEffect) return;

    track.UndoEffect();
    AfterEdit(track);
  }



  // Helper methods
  private Panel CreateTrackPanel(Track track)
  {
    DrawingPanel pan = new()
    {
      BackColor = track.TrackColor.IsEmpty ? Settings.Color.TrackBg_Unselected : track.TrackColor,
      Width = panTracks.Width,
      Left = 0,
      Top = 0,
      BorderStyle = BorderStyle.None
    };

    pan.Click += (sender, e) => SelectTrack(track);
    pan.MouseDown += (sender, e) =>
    {
      SelectTrack(track);
      selectionStart = selectionEnd = e.X + firstVisiblePixel;
      pan.Invalidate();
      UpdatePlaybackView();
    };
    pan.MouseMove += (sender, e) =>
    {
      if (e.Button == MouseButtons.Left)
      {
        selectionEnd = e.X + firstVisiblePixel;
        pan.Invalidate();
        UpdatePlaybackView();
      }
    };
    pan.DoubleClick += (sender, e) =>
    {
      track.ToggleDisabled();
      pan.Invalidate();
    };
    pan.Paint += (sender, e) =>
    {
      GraphicsState state = e.Graphics.Save();
      e.Graphics.TranslateTransform(-firstVisiblePixel, 0);
      track.DrawSamples(e.Graphics, trackManager.DrawTimescale);
      if (track == GetSelectedTrack() && selectionStart != selectionEnd)
      {
        track.ShowSelection(e.Graphics, Math.Min(selectionStart, selectionEnd), Math.Max(selectionStart, selectionEnd));
      }
      e.Graphics.Restore(state);
      track.DrawTrackHeader(e.Graphics);
    };

    ContextMenuStrip colorMenu = new() { ShowImageMargin = false };
    colorMenu.Items.Add("Change track color...", null, (sender, e) =>
    {
      SelectTrack(track);
      using ColorDialog dialog = new() { Color = track.TrackColor, FullOpen = true };
      if (dialog.ShowDialog(this) == DialogResult.OK) track.SetTrackColor(dialog.Color);
    });
    colorMenu.Items.Add(new ToolStripSeparator());
    colorMenu.Items.Add("Delete Track...", null, (sender, e) =>
    {
      if (MessageBox.Show(this, "Delete this track? This cannot be undone.", "Delete Track", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
      {
        RemoveTrack(track);
      }
    });
    pan.ContextMenuStrip = colorMenu;
    pan.Disposed += (sender, e) => colorMenu.Dispose();

    panTracks.Controls.Add(pan);
    track.LinkPanels(pan, panTracks);
    SelectTrack(track);
    return pan;
  }

  private void HideMenuImageMargins(ToolStripItemCollection items)
  {
    foreach (ToolStripItem item in items)
    {
      if (item is ToolStripMenuItem menuItem)
      {
        if (menuItem.DropDown is ToolStripDropDownMenu dropDown)
        {
          dropDown.ShowImageMargin = false;
        }
        HideMenuImageMargins(menuItem.DropDownItems);
      }
    }
  }

  private Track GetSelectedTrack()
  {
    if (trackManager is null || trackManager.Tracks.Count == 0) return null;
    return trackManager.Tracks.Contains(trackManager.SelectedTrack) ? trackManager.SelectedTrack : trackManager.Tracks[0];
  }

  private void SelectTrack(Track track)
  {
    if (track is null) return;
    if (trackManager.SelectedTrack != track)
    {
      selectionStart = selectionEnd = 0;
    }
    if (playbackTrack is not null && playbackTrack != track)
    {
      playbackTrack.Pause();
      playbackTrack = null;
      firstVisiblePixel = 0;
    }
    trackManager.SelectTrack(track);
    foreach (Track item in trackManager.Tracks) item.PanDrawSamples.BorderStyle = BorderStyle.None;
    UpdatePlaybackView();
  }

  private void RemoveTrack(Track track)
  {
    if (track is null) return;
    trackManager.ResetAll();
    playbackTrack = null;
    firstVisiblePixel = 0;
    playheadPixel = 0;
    selectionStart = selectionEnd = 0;
    bool wasSelected = trackManager.SelectedTrack == track;
    trackManager.RemTrack(track);
    panTracks.Controls.Remove(track.PanDrawSamples);
    track.PanDrawSamples.Dispose();
    track.Dispose();
    if (wasSelected && trackManager.Tracks.Count > 0) trackManager.SelectTrack(trackManager.Tracks[0]);
    UpdateSizes();
  }

  private void menuEditTrim_Click(object sender, EventArgs e)
  {
    Track track = GetSelectedTrack();
    if (track is null || selectionStart == selectionEnd) return;
    track.Trim(Math.Min(selectionStart, selectionEnd), Math.Max(selectionStart, selectionEnd), trackManager.DrawTimescale);
    AfterEdit(track);
  }

  private void menuEditDeleteSel_Click(object sender, EventArgs e)
  {
    Track track = GetSelectedTrack();
    if (track is null || selectionStart == selectionEnd) return;
    track.DeletePart(Math.Min(selectionStart, selectionEnd), Math.Max(selectionStart, selectionEnd), trackManager.DrawTimescale);
    AfterEdit(track);
  }

  private void AfterEdit(Track track)
  {
    selectionStart = selectionEnd = 0;
    trackManager.ResetAll();
    playbackTrack = null;
    firstVisiblePixel = 0;
    playheadPixel = 0;
    track.PanDrawSamples.Invalidate();
    UpdatePlaybackView();
  }

  private void UpdatePlaybackView()
  {
    if (trackManager is null || panTimeRuler is null || hsbTimeline is null) return;
    bool hasTracks = trackManager.Tracks.Count > 0;
    bool hasSelection = hasTracks && GetSelectedTrack() is not null && selectionStart != selectionEnd;
    btnPlay.Enabled = hasTracks;
    buttonReset.Enabled = hasTracks;
    buttonTrim.Enabled = hasSelection;
    buttonDeleteSelection.Enabled = hasSelection;
    btnPlay.Visible = true;
    bool isPlaying = trackManager.State == NAudio.Wave.PlaybackState.Playing;
    btnPlay.ImageIndex = isPlaying ? 5 : 4;
    topButtonTips.SetToolTip(btnPlay, isPlaying ? "Pause" : "Play");

    UpdateTimelineScrollBar();
    if (hasTracks)
    {
      playheadPixel = trackManager.GetPlayheadPos();
      int visibleWidth = Math.Max(1, panTracks.ClientSize.Width - 20);
      int oldStart = firstVisiblePixel;

      // Follow playback automatically, but let the user scroll freely while paused or stopped.
      if (isPlaying)
      {
        if (playheadPixel < visibleWidth) firstVisiblePixel = 0;
        else if (playheadPixel >= firstVisiblePixel + visibleWidth)
          firstVisiblePixel = Math.Max(0, playheadPixel - visibleWidth / 4);
      }

      UpdateTimelineScrollBar();
      if (oldStart != firstVisiblePixel) trackManager.RedrawAll();
    }

    if (hasTracks)
    {
      int currentPlayheadX = playheadPixel - firstVisiblePixel;
      if (lastPlayheadScreenX != currentPlayheadX)
      {
        InvalidatePlayheadColumn(lastPlayheadScreenX);
        InvalidatePlayheadColumn(currentPlayheadX);
      }

      lastPlayheadScreenX = currentPlayheadX;
      lblPlayhead.Left = currentPlayheadX;
      lblPlayhead.Visible = !isZooming;
      lblPlayhead.BringToFront();
    }
    else
    {
      InvalidatePlayheadColumn(lastPlayheadScreenX);
      lblPlayhead.Visible = false;
      lastPlayheadScreenX = -1;
    }

    panTimeRuler.Invalidate();
  }

  private void InvalidatePlayheadColumn(int x)
  {
    if (panTracks is null || x < 0 || x >= panTracks.ClientSize.Width)
    {
      return;
    }

    int left = Math.Max(0, x - 1);
    int right = Math.Min(panTracks.ClientSize.Width, x + 2);
    panTracks.Invalidate(new Rectangle(left, 0, right - left, panTracks.ClientSize.Height), true);
  }

  private void hsbTimeline_Scroll(object sender, ScrollEventArgs e)
  {
    firstVisiblePixel = hsbTimeline.Value;
    trackManager.RedrawAll();
    panTimeRuler.Invalidate();
    UpdatePlaybackView();
  }

  private void UpdateTimelineScrollBar()
  {
    if (hsbTimeline is null || trackManager is null || panTracks is null) return;

    int visibleWidth = Math.Max(1, panTracks.ClientSize.Width);
    int scale = Math.Max(1, trackManager.DrawTimescale);
    int contentWidth = 0;

    foreach (Track track in trackManager.Tracks)
    {
      int trackWidth = track.DrawOffset +
        (int)Math.Ceiling(track.GetDurationSeconds() * track.SampleValuesPerSecond / scale);
      contentWidth = Math.Max(contentWidth, trackWidth);
    }

    int maxScroll = Math.Max(0, contentWidth - visibleWidth);
    hsbTimeline.LargeChange = visibleWidth;
    hsbTimeline.SmallChange = Math.Max(1, visibleWidth / 20);
    hsbTimeline.Maximum = maxScroll + visibleWidth - 1;
    hsbTimeline.Visible = maxScroll > 0;

    firstVisiblePixel = Math.Clamp(firstVisiblePixel, 0, maxScroll);
    hsbTimeline.Value = firstVisiblePixel;
  }

  private void RebuildTrackPanels()
  {
    List<Track> loadedTracks = new(trackManager.Tracks);
    trackManager.Tracks.Clear();

    foreach (Track track in loadedTracks)
    {
      CreateTrackPanel(track);
      trackManager.AddTrack(track);
      SelectTrack(track);
    }

    UpdateSizes();
    trackManager.RedrawAll();
  }
  private void UpdateSizes()
  {
    if (trackManager is null || panTimeRuler is null) return;

    int padding = 15 * DeviceDpi / 96;
    int contentWidth = Math.Max(1, ClientSize.Width - padding * 2);

    int gap = 14;
    int rowWidth = buttonDrums.Width + buttonGuitar.Width + buttonPiano.Width + buttonSynth.Width + gap * 3;
    int left = (ClientSize.Width - rowWidth) / 2;
    int instrumentsY = Math.Min(935 * DeviceDpi / 96, ClientSize.Height - buttonDrums.Height - 20 * DeviceDpi / 96);

    lbInstPanel.Size = new Size(rowWidth, 24 * DeviceDpi / 96);
    lbInstPanel.Location = new Point(left, instrumentsY - lbInstPanel.Height - 10 * DeviceDpi / 96);
    buttonDrums.Location = new Point(left, instrumentsY);
    buttonGuitar.Location = new Point(left + buttonDrums.Width + gap, instrumentsY);
    buttonPiano.Location = new Point(left + buttonDrums.Width + buttonGuitar.Width + gap * 2, instrumentsY);
    buttonSynth.Location = new Point(left + buttonDrums.Width + buttonGuitar.Width + buttonPiano.Width + gap * 3, instrumentsY);

    trkDrawTimescale.Left = padding;
    trkDrawTimescale.Width = contentWidth;

    int rulerTop = trkDrawTimescale.Bottom + 4;
    panTimeRuler.SetBounds(padding, rulerTop, contentWidth, 32 * DeviceDpi / 96);

    int tracksTop = panTimeRuler.Bottom + 4;
    int scrollbarHeight = SystemInformation.HorizontalScrollBarHeight;
    int scrollbarGap = 3 * DeviceDpi / 96;
    int tracksBottom = lbInstPanel.Top - scrollbarHeight - scrollbarGap;
    panTracks.SetBounds(padding, tracksTop, contentWidth, Math.Max(1, tracksBottom - tracksTop));
    hsbTimeline.SetBounds(padding, tracksBottom + scrollbarGap, contentWidth, scrollbarHeight);

    int count = trackManager.Tracks.Count;
    for (int i = 0; i < count; i++)
    {
      int top = i * panTracks.ClientSize.Height / count;
      int bottom = (i + 1) * panTracks.ClientSize.Height / count;
      Panel panel = trackManager.Tracks[i].PanDrawSamples;
      panel.SetBounds(0, top, panTracks.ClientSize.Width, Math.Max(1, bottom - top - 1));
      panel.Invalidate();
    }

    lblPlayhead.Height = panTracks.ClientSize.Height;
    lblPlayhead.BringToFront();
    panTimeRuler.BringToFront();
    trkDrawTimescale.BringToFront();
    hsbTimeline.BringToFront();

    int toolbarGap = 10;
    Button[] toolbarButtons = { btnPlay, buttonReset, buttonRecord, buttonImport, buttonTrim, buttonDeleteSelection };
    int toolbarWidth = toolbarButtons.Sum(button => button.Width) + toolbarGap * (toolbarButtons.Length - 1);
    int toolbarLeft = (ClientSize.Width - toolbarWidth) / 2;
    int toolbarTop = 39 * DeviceDpi / 96;
    int toolbarX = toolbarLeft;
    foreach (Button button in toolbarButtons)
    {
      button.Location = new Point(toolbarX, toolbarTop);
      toolbarX += button.Width + toolbarGap;
    }

    UpdatePlaybackView();
  }
  private void DrawTimeRuler(object sender, PaintEventArgs e)
  {
    if (trackManager is null || panTimeRuler.Width < 20) return;

    Track referenceTrack = playbackTrack ?? GetSelectedTrack();
    int valuesPerSecond = referenceTrack?.SampleValuesPerSecond ?? 44100;
    double pixelsPerSecond = (double)valuesPerSecond / Math.Max(1, trackManager.DrawTimescale);
    if (pixelsPerSecond <= 0) return;

    double[] intervals = { 0.001, 0.002, 0.005, 0.01, 0.02, 0.05, 0.1, 0.2, 0.5, 1, 2, 5, 10, 15, 30, 60, 120, 300, 600, 1800, 3600 };
    double majorStep = intervals[^1];
    foreach (double interval in intervals)
    {
      if (interval * pixelsPerSecond >= 90 * DeviceDpi / 96.0) { majorStep = interval; break; }
    }

    double minorStep = majorStep / 5;
    double tickSpacing = minorStep * pixelsPerSecond;
    int originX = panTimeRuler.PointToClient(panTracks.PointToScreen(Point.Empty)).X - firstVisiblePixel;
    int bottom = panTimeRuler.Height - 1;
    using Pen pen = new(panTimeRuler.ForeColor);
    e.Graphics.DrawLine(pen, 0, bottom, panTimeRuler.Width, bottom);

    int firstTick = Math.Max(0, (int)Math.Ceiling(-originX / tickSpacing));
    for (int i = firstTick; ; i++)
    {
      double seconds = i * minorStep;
      float x = (float)(originX + seconds * pixelsPerSecond);
      if (x >= panTimeRuler.Width) break;
      bool major = i % 5 == 0;
      e.Graphics.DrawLine(pen, x, bottom, x, bottom - (major ? 12 : 6));
      if (!major) continue;

      TimeSpan time = TimeSpan.FromSeconds(Math.Round(seconds, 3));
      string label = time.TotalHours >= 1 ? ((int)time.TotalHours) + time.ToString(@"\:mm\:ss") : time.ToString(@"m\:ss");
      if (majorStep < 1) label += time.ToString(@"\.fff");
      Size size = TextRenderer.MeasureText(label, panTimeRuler.Font, Size.Empty, TextFormatFlags.NoPadding);
      int labelX = Math.Max(0, (int)x - size.Width / 2);
      if (labelX + size.Width <= panTimeRuler.Width)
        TextRenderer.DrawText(e.Graphics, label, panTimeRuler.Font, new Point(labelX, 2), panTimeRuler.ForeColor, TextFormatFlags.NoPadding);
    }

    if (trackManager.Tracks.Count > 0)
    {
      int markerX = originX + playheadPixel;
      int size = 7 * DeviceDpi / 96;
      Point[] arrow = { new(markerX - size, bottom - size), new(markerX + size, bottom - size), new(markerX, bottom) };
      e.Graphics.FillPolygon(Brushes.DodgerBlue, arrow);
    }
  }

  private void flpInstruments_Paint(object sender, PaintEventArgs e)
  {

  }


  private void buttonSynth_Click(object sender, EventArgs e)
  {
    using FrmGenerateSynth dialog = new();

    if (dialog.ShowDialog(this) != DialogResult.OK)
      return;

    try
    {
      Track newTrack = SoundFontGenerator.CreateSynthTrack(
        dialog.Instrument,
        dialog.Note,
        dialog.Octave,
        dialog.DurationSeconds
      );

      newTrack.SetDisplayName(
        $"{dialog.Instrument} {dialog.Note}{dialog.Octave}"
      );

      newTrack.DrawOffset = (int)Math.Round(
        dialog.StartTimeSeconds *
        newTrack.SampleValuesPerSecond /
        Math.Max(1, trackManager.DrawTimescale)
      );

      CreateTrackPanel(newTrack);
      trackManager.AddTrack(newTrack);
      SelectTrack(newTrack);
      UpdateSizes();
      newTrack.PanDrawSamples.Invalidate();
    }
    catch (Exception ex)
    {
      MessageBox.Show(
        ex.Message, "Synth",
        MessageBoxButtons.OK, MessageBoxIcon.Error
      );
    }
  }

  // Keep the existing Designer click connection if it still points here.
  private void buttonTone_Click(object sender, EventArgs e)
  {
    buttonSynth_Click(sender, e);
  }



  private void buttonGuitar_Click(object sender, EventArgs e)
  {
    using FrmGeneratePluck dialog = new();

    if (dialog.ShowDialog(this) != DialogResult.OK)
      return;

    try
    {
      Track newTrack = SoundFontGenerator.CreatePluckTrack(
        dialog.Instrument,
        dialog.Note,
        dialog.Octave,
        dialog.DurationSeconds
      );

      newTrack.SetDisplayName(
        $"{dialog.Instrument} {dialog.Note}{dialog.Octave}"
      );

      newTrack.DrawOffset = (int)Math.Round(
        dialog.StartTimeSeconds *
        newTrack.SampleValuesPerSecond /
        Math.Max(1, trackManager.DrawTimescale)
      );

      CreateTrackPanel(newTrack);
      trackManager.AddTrack(newTrack);
      SelectTrack(newTrack);
      UpdateSizes();
      newTrack.PanDrawSamples.Invalidate();
    }
    catch (Exception ex)
    {
      MessageBox.Show(
        ex.Message, "Pluck",
        MessageBoxButtons.OK, MessageBoxIcon.Error
      );
    }
  }





  private void buttonPiano_Click(object sender, EventArgs e)
  {
    using FrmGenerateChord dialog = new();

    if (dialog.ShowDialog(this) != DialogResult.OK)
      return;

    try
    {
      Track newTrack = SoundFontGenerator.CreateChordTrack(
        dialog.RootNote,
        dialog.ChordType,
        dialog.Octave,
        dialog.DurationSeconds
      );

      newTrack.SetDisplayName(
        $"{dialog.RootNote} {dialog.ChordType} Chord"
      );

      newTrack.DrawOffset = (int)Math.Round(
        dialog.StartTimeSeconds *
        newTrack.SampleValuesPerSecond /
        Math.Max(1, trackManager.DrawTimescale)
      );

      CreateTrackPanel(newTrack);
      trackManager.AddTrack(newTrack);
      SelectTrack(newTrack);
      UpdateSizes();
      newTrack.PanDrawSamples.Invalidate();
    }
    catch (Exception ex)
    {
      MessageBox.Show(
        ex.Message, "Chord",
        MessageBoxButtons.OK, MessageBoxIcon.Error
      );
    }
  }




  private void buttonDrums_Click(object sender, EventArgs e)
  {
    using FrmGenerateBeat dialog = new();

    if (dialog.ShowDialog(this) != DialogResult.OK)
      return;

    try
    {
      Track newTrack = SoundFontGenerator.CreateBeatTrack(
        dialog.DrumKit,
        dialog.Rhythm,
        dialog.Tempo,
        dialog.Bars
      );

      newTrack.SetDisplayName(
        $"{dialog.DrumKit} {dialog.Rhythm}"
      );

      newTrack.DrawOffset = (int)Math.Round(
        dialog.StartTimeSeconds *
        newTrack.SampleValuesPerSecond /
        Math.Max(1, trackManager.DrawTimescale)
      );

      CreateTrackPanel(newTrack);
      trackManager.AddTrack(newTrack);
      SelectTrack(newTrack);
      UpdateSizes();
      newTrack.PanDrawSamples.Invalidate();
    }
    catch (Exception ex)
    {
      MessageBox.Show(
        ex.Message, "Beat",
        MessageBoxButtons.OK, MessageBoxIcon.Error
      );
    }
  }


  private void lbInstPanel_TextChanged(object sender, EventArgs e)
  {

  }
  private void buttonReset_Click(object sender, EventArgs e)
  {
    trackManager.ResetAll();
    playbackTrack = null;
    firstVisiblePixel = 0;
    playheadPixel = 0;
    trackManager.RedrawAll();
    UpdatePlaybackView();
  }
  private void saveToolStripMenuItem_Click(object sender, EventArgs e)
  {
    SaveCurrentProject(false);
  }

  private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
  {
    SaveCurrentProject(true);
  }

  private void openToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (!ConfirmSaveBeforeReplacingProject()) return;

    using OpenFileDialog dialog = new()
    {
      Filter = "Sound Designer Project (*.soundproj)|*.soundproj",
      DefaultExt = "soundproj"
    };

    if (dialog.ShowDialog() != DialogResult.OK)
    {
      return;
    }

    try
    {
      playbackTrack = null;
      firstVisiblePixel = 0;
      trackManager.LoadProject(dialog.FileName);
      RebuildTrackPanels();
      currentProjectPath = dialog.FileName;
    }
    catch (Exception ex)
    {
      ShowOperationError("open the project", ex);
    }
  }

  private void menuFileNewProject_Click(object sender, EventArgs e)
  {
    if (!ConfirmSaveBeforeReplacingProject()) return;

    playbackTrack = null;
    firstVisiblePixel = 0;
    trackManager.NewProject();
    currentProjectPath = null;
    UpdateSizes();
  }
}

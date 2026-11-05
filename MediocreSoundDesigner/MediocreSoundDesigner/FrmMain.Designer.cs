namespace MediocreSoundDesigner
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
      panTracks = new Panel();
      lblPlayhead = new Label();
      btnPlay = new Button();
      lblDebug = new Label();
      ofdLoadFile = new OpenFileDialog();
      menu = new MenuStrip();
      menuFile = new ToolStripMenuItem();
      newToolStripMenuItem = new ToolStripMenuItem();
      openToolStripMenuItem = new ToolStripMenuItem();
      saveToolStripMenuItem = new ToolStripMenuItem();
      saveAsToolStripMenuItem = new ToolStripMenuItem();
      menuFileOpen = new ToolStripMenuItem();
      menuFileSave = new ToolStripMenuItem();
      menuEdit = new ToolStripMenuItem();
      menuEditFlip = new ToolStripMenuItem();
      menuEditReverse = new ToolStripMenuItem();
      menuEditVolume = new ToolStripMenuItem();
      menuEditEliminateSilence = new ToolStripMenuItem();
      menuGen = new ToolStripMenuItem();
      menuGenNoise = new ToolStripMenuItem();
      menuGenGradientNoise = new ToolStripMenuItem();
      tmrUpdate = new System.Windows.Forms.Timer(components);
      sfdSaveFile = new SaveFileDialog();
      trkDrawTimescale = new TrackBar();
      colorDialog = new ColorDialog();
      buttonRecord = new Button();
      buttonTrim = new Button();
      buttonGuitar = new Button();
      buttonSynth = new Button();
      buttonDrums = new Button();
      buttonPiano = new Button();
      lbGeneratorPanel = new TextBox();
      imageList1 = new ImageList(components);
      buttonReset = new Button();
      buttonDeleteSelection = new Button();
      buttonImport = new Button();
      panTracks.SuspendLayout();
      menu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).BeginInit();
      SuspendLayout();
      // 
      // panTracks
      // 
      panTracks.AutoScroll = true;
      panTracks.BackColor = Color.DimGray;
      panTracks.BorderStyle = BorderStyle.Fixed3D;
      panTracks.Controls.Add(lblPlayhead);
      panTracks.Location = new Point(9, 174);
      panTracks.Margin = new Padding(3, 4, 3, 4);
      panTracks.Name = "panTracks";
      panTracks.Size = new Size(1805, 720);
      panTracks.TabIndex = 0;
      // 
      // lblPlayhead
      // 
      lblPlayhead.BackColor = Color.White;
      lblPlayhead.Location = new Point(45, -3);
      lblPlayhead.Name = "lblPlayhead";
      lblPlayhead.Size = new Size(6, 513);
      lblPlayhead.TabIndex = 9;
      // 
      // btnPlay
      // 
      btnPlay.AutoSize = true;
      btnPlay.BackColor = Color.Transparent;
      btnPlay.BackgroundImage = (Image)resources.GetObject("btnPlay.BackgroundImage");
      btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
      btnPlay.FlatAppearance.BorderSize = 0;
      btnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent;
      btnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent;
      btnPlay.FlatStyle = FlatStyle.Flat;
      btnPlay.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      btnPlay.Location = new Point(523, 44);
      btnPlay.Margin = new Padding(3, 4, 3, 4);
      btnPlay.Name = "btnPlay";
      btnPlay.Size = new Size(40, 40);
      btnPlay.TabIndex = 6;
      btnPlay.UseVisualStyleBackColor = false;
      btnPlay.Click += btnPlay_Click;
      // 
      // lblDebug
      // 
      lblDebug.Dock = DockStyle.Bottom;
      lblDebug.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblDebug.ForeColor = Color.FromArgb(255, 255, 192);
      lblDebug.Location = new Point(0, 995);
      lblDebug.Name = "lblDebug";
      lblDebug.Size = new Size(1833, 49);
      lblDebug.TabIndex = 1;
      // 
      // ofdLoadFile
      // 
      ofdLoadFile.FileName = "openFileDialog1";
      // 
      // menu
      // 
      menu.ImageScalingSize = new Size(20, 20);
      menu.Items.AddRange(new ToolStripItem[] { menuFile, menuEdit, menuGen });
      menu.Location = new Point(0, 0);
      menu.Name = "menu";
      menu.Padding = new Padding(7, 3, 0, 3);
      menu.Size = new Size(1833, 30);
      menu.TabIndex = 5;
      menu.Text = "menuStrip1";
      // 
      // menuFile
      // 
      menuFile.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, menuFileOpen, menuFileSave });
      menuFile.Name = "menuFile";
      menuFile.Size = new Size(46, 24);
      menuFile.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      newToolStripMenuItem.Name = "newToolStripMenuItem";
      newToolStripMenuItem.Size = new Size(240, 26);
      newToolStripMenuItem.Text = "New";
      newToolStripMenuItem.Click += menuFileNewProject_Click;
      // 
      // openToolStripMenuItem
      // 
      openToolStripMenuItem.Name = "openToolStripMenuItem";
      openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
      openToolStripMenuItem.Size = new Size(240, 26);
      openToolStripMenuItem.Text = "Open Project...";
      openToolStripMenuItem.Click += openToolStripMenuItem_Click;
      // 
      // saveToolStripMenuItem
      // 
      saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
      saveToolStripMenuItem.Size = new Size(240, 26);
      saveToolStripMenuItem.Text = "Save Project";
      saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
      // 
      // saveAsToolStripMenuItem
      // 
      saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      saveAsToolStripMenuItem.Size = new Size(240, 26);
      saveAsToolStripMenuItem.Text = "Save Project As...";
      saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
      // 
      // menuFileOpen
      // 
      menuFileOpen.Name = "menuFileOpen";
      menuFileOpen.Size = new Size(240, 26);
      menuFileOpen.Text = "Import...";
      menuFileOpen.Click += menuFileOpen_Click;
      // 
      // menuFileSave
      // 
      menuFileSave.Name = "menuFileSave";
      menuFileSave.Size = new Size(240, 26);
      menuFileSave.Text = "Export...";
      menuFileSave.Click += menuFileSave_Click;
      // 
      // menuEdit
      // 
      menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditFlip, menuEditReverse, menuEditVolume, menuEditEliminateSilence });
      menuEdit.Name = "menuEdit";
      menuEdit.Size = new Size(49, 24);
      menuEdit.Text = "E&dit";
      // 
      // menuEditFlip
      // 
      menuEditFlip.Name = "menuEditFlip";
      menuEditFlip.Size = new Size(214, 26);
      menuEditFlip.Text = "Flip";
      menuEditFlip.Click += menuEditFlip_Click;
      // 
      // menuEditReverse
      // 
      menuEditReverse.Name = "menuEditReverse";
      menuEditReverse.Size = new Size(214, 26);
      menuEditReverse.Text = "Reverse";
      menuEditReverse.Click += menuEditReverse_Click;
      // 
      // menuEditVolume
      // 
      menuEditVolume.Name = "menuEditVolume";
      menuEditVolume.Size = new Size(214, 26);
      menuEditVolume.Text = "Alter Volume...";
      menuEditVolume.Click += menuEditVolume_Click;
      // 
      // menuEditEliminateSilence
      // 
      menuEditEliminateSilence.Name = "menuEditEliminateSilence";
      menuEditEliminateSilence.Size = new Size(214, 26);
      menuEditEliminateSilence.Text = "Eliminate Silence...";
      menuEditEliminateSilence.Click += menuEditEliminateSilence_Click;
      // 
      // menuGen
      // 
      menuGen.DropDownItems.AddRange(new ToolStripItem[] { menuGenNoise, menuGenGradientNoise });
      menuGen.Name = "menuGen";
      menuGen.Size = new Size(83, 24);
      menuGen.Text = "&Generate";
      // 
      // menuGenNoise
      // 
      menuGenNoise.Name = "menuGenNoise";
      menuGenNoise.Size = new Size(191, 26);
      menuGenNoise.Text = "Noise";
      menuGenNoise.Click += menuGenNoise_Click;
      // 
      // menuGenGradientNoise
      // 
      menuGenGradientNoise.Name = "menuGenGradientNoise";
      menuGenGradientNoise.Size = new Size(191, 26);
      menuGenGradientNoise.Text = "Gradient Noise";
      menuGenGradientNoise.Click += menuGenGradientNoise_Click;
      // 
      // tmrUpdate
      // 
      tmrUpdate.Enabled = true;
      tmrUpdate.Interval = 20;
      tmrUpdate.Tick += tmrUpdatePlaybackState_Tick;
      // 
      // trkDrawTimescale
      // 
      trkDrawTimescale.LargeChange = 50;
      trkDrawTimescale.Location = new Point(12, 111);
      trkDrawTimescale.Margin = new Padding(3, 4, 3, 4);
      trkDrawTimescale.Maximum = 1000;
      trkDrawTimescale.Minimum = 1;
      trkDrawTimescale.Name = "trkDrawTimescale";
      trkDrawTimescale.Size = new Size(1806, 56);
      trkDrawTimescale.SmallChange = 10;
      trkDrawTimescale.TabIndex = 7;
      trkDrawTimescale.TickFrequency = 10;
      trkDrawTimescale.Value = 500;
      trkDrawTimescale.Scroll += trkDrawTimescale_Scroll;
      // 
      // buttonRecord
      // 
      buttonRecord.BackColor = Color.Transparent;
      buttonRecord.BackgroundImage = (Image)resources.GetObject("buttonRecord.BackgroundImage");
      buttonRecord.BackgroundImageLayout = ImageLayout.Stretch;
      buttonRecord.FlatStyle = FlatStyle.Flat;
      buttonRecord.Location = new Point(640, 39);
      buttonRecord.Name = "buttonRecord";
      buttonRecord.Size = new Size(45, 45);
      buttonRecord.TabIndex = 7;
      buttonRecord.UseVisualStyleBackColor = false;
      buttonRecord.Click += menuGenRecord_Click;
      // 
      // buttonTrim
      // 
      buttonTrim.BackgroundImage = (Image)resources.GetObject("buttonTrim.BackgroundImage");
      buttonTrim.BackgroundImageLayout = ImageLayout.Stretch;
      buttonTrim.Enabled = false;
      buttonTrim.FlatStyle = FlatStyle.Flat;
      buttonTrim.Location = new Point(579, 39);
      buttonTrim.Name = "buttonTrim";
      buttonTrim.Size = new Size(45, 45);
      buttonTrim.TabIndex = 8;
      buttonTrim.UseVisualStyleBackColor = true;
      buttonTrim.Click += menuEditTrim_Click;
      // 
      // buttonGuitar
      // 
      buttonGuitar.BackColor = Color.FromArgb(52, 52, 52);
      buttonGuitar.BackgroundImage = (Image)resources.GetObject("buttonGuitar.BackgroundImage");
      buttonGuitar.BackgroundImageLayout = ImageLayout.Stretch;
      buttonGuitar.Enabled = false;
      buttonGuitar.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
      buttonGuitar.FlatStyle = FlatStyle.Flat;
      buttonGuitar.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
      buttonGuitar.ForeColor = Color.White;
      buttonGuitar.Location = new Point(809, 935);
      buttonGuitar.Name = "buttonGuitar";
      buttonGuitar.Size = new Size(100, 85);
      buttonGuitar.TabIndex = 9;
      buttonGuitar.TextAlign = ContentAlignment.BottomCenter;
      buttonGuitar.TextImageRelation = TextImageRelation.ImageAboveText;
      buttonGuitar.UseVisualStyleBackColor = false;
      buttonGuitar.Click += buttonGuitar_Click;
      // 
      // buttonSynth
      // 
      buttonSynth.BackColor = Color.FromArgb(52, 52, 52);
      buttonSynth.BackgroundImage = (Image)resources.GetObject("buttonSynth.BackgroundImage");
      buttonSynth.BackgroundImageLayout = ImageLayout.Stretch;
      buttonSynth.Enabled = false;
      buttonSynth.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
      buttonSynth.FlatStyle = FlatStyle.Flat;
      buttonSynth.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
      buttonSynth.ForeColor = Color.White;
      buttonSynth.Location = new Point(1041, 935);
      buttonSynth.Name = "buttonSynth";
      buttonSynth.Size = new Size(100, 85);
      buttonSynth.TabIndex = 12;
      buttonSynth.TextAlign = ContentAlignment.BottomCenter;
      buttonSynth.TextImageRelation = TextImageRelation.ImageAboveText;
      buttonSynth.UseVisualStyleBackColor = false;
      buttonSynth.Click += this.buttonTone_Click;
      // 
      // buttonDrums
      // 
      buttonDrums.BackColor = Color.FromArgb(52, 52, 52);
      buttonDrums.BackgroundImage = (Image)resources.GetObject("buttonDrums.BackgroundImage");
      buttonDrums.BackgroundImageLayout = ImageLayout.Stretch;
      buttonDrums.Enabled = false;
      buttonDrums.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
      buttonDrums.FlatStyle = FlatStyle.Flat;
      buttonDrums.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
      buttonDrums.ForeColor = Color.White;
      buttonDrums.Location = new Point(693, 935);
      buttonDrums.Name = "buttonDrums";
      buttonDrums.Size = new Size(100, 85);
      buttonDrums.TabIndex = 10;
      buttonDrums.TextAlign = ContentAlignment.BottomCenter;
      buttonDrums.TextImageRelation = TextImageRelation.ImageAboveText;
      buttonDrums.UseVisualStyleBackColor = false;
      buttonDrums.Click += buttonDrums_Click;
      // 
      // buttonPiano
      // 
      buttonPiano.BackColor = Color.FromArgb(52, 52, 52);
      buttonPiano.BackgroundImage = (Image)resources.GetObject("buttonPiano.BackgroundImage");
      buttonPiano.BackgroundImageLayout = ImageLayout.Stretch;
      buttonPiano.Enabled = false;
      buttonPiano.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
      buttonPiano.FlatStyle = FlatStyle.Flat;
      buttonPiano.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
      buttonPiano.ForeColor = Color.White;
      buttonPiano.Location = new Point(925, 935);
      buttonPiano.Name = "buttonPiano";
      buttonPiano.Size = new Size(100, 85);
      buttonPiano.TabIndex = 11;
      buttonPiano.TextAlign = ContentAlignment.BottomCenter;
      buttonPiano.TextImageRelation = TextImageRelation.ImageAboveText;
      buttonPiano.UseVisualStyleBackColor = false;
      buttonPiano.Click += buttonPiano_Click;
      // 
      // lbGeneratorPanel
      // 
      lbGeneratorPanel.BackColor = Color.Black;
      lbGeneratorPanel.BorderStyle = BorderStyle.None;
      lbGeneratorPanel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
      lbGeneratorPanel.ForeColor = Color.White;
      lbGeneratorPanel.Location = new Point(900, 899);
      lbGeneratorPanel.Name = "lbGeneratorPanel";
      lbGeneratorPanel.Size = new Size(110, 23);
      lbGeneratorPanel.TabIndex = 14;
      lbGeneratorPanel.Text = "Generators";
      lbGeneratorPanel.TextAlign = HorizontalAlignment.Center;
      lbGeneratorPanel.TextChanged += lbInstPanel_TextChanged;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth32Bit;
      imageList1.ImageSize = new Size(36, 36);
      imageList1.TransparentColor = Color.Transparent;
      // 
      // buttonReset
      // 
      buttonReset.BackgroundImage = (Image)resources.GetObject("buttonReset.BackgroundImage");
      buttonReset.BackgroundImageLayout = ImageLayout.Stretch;
      buttonReset.Enabled = false;
      buttonReset.FlatStyle = FlatStyle.Flat;
      buttonReset.Location = new Point(702, 39);
      buttonReset.Name = "buttonReset";
      buttonReset.Size = new Size(45, 45);
      buttonReset.TabIndex = 17;
      buttonReset.UseVisualStyleBackColor = true;
      buttonReset.Click += buttonReset_Click;
      // 
      // buttonDeleteSelection
      // 
      buttonDeleteSelection.BackgroundImage = (Image)resources.GetObject("buttonDeleteSelection.BackgroundImage");
      buttonDeleteSelection.BackgroundImageLayout = ImageLayout.Stretch;
      buttonDeleteSelection.Enabled = false;
      buttonDeleteSelection.FlatStyle = FlatStyle.Flat;
      buttonDeleteSelection.Location = new Point(762, 39);
      buttonDeleteSelection.Name = "buttonDeleteSelection";
      buttonDeleteSelection.Size = new Size(45, 45);
      buttonDeleteSelection.TabIndex = 18;
      buttonDeleteSelection.UseVisualStyleBackColor = true;
      buttonDeleteSelection.Click += menuEditDeleteSel_Click;
      // 
      // buttonImport
      // 
      buttonImport.AutoSize = true;
      buttonImport.BackColor = Color.Transparent;
      buttonImport.BackgroundImage = (Image)resources.GetObject("buttonImport.BackgroundImage");
      buttonImport.BackgroundImageLayout = ImageLayout.Stretch;
      buttonImport.FlatAppearance.BorderSize = 0;
      buttonImport.FlatAppearance.MouseDownBackColor = Color.Transparent;
      buttonImport.FlatAppearance.MouseOverBackColor = Color.Transparent;
      buttonImport.FlatStyle = FlatStyle.Flat;
      buttonImport.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      buttonImport.Location = new Point(465, 44);
      buttonImport.Margin = new Padding(3, 4, 3, 4);
      buttonImport.Name = "buttonImport";
      buttonImport.Size = new Size(40, 40);
      buttonImport.TabIndex = 19;
      buttonImport.UseVisualStyleBackColor = false;
      buttonImport.Click += menuFileOpen_Click;
      // 
      // FrmMain
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(1833, 1044);
      Controls.Add(buttonImport);
      Controls.Add(buttonDeleteSelection);
      Controls.Add(buttonReset);
      Controls.Add(lbGeneratorPanel);
      Controls.Add(buttonRecord);
      Controls.Add(buttonPiano);
      Controls.Add(buttonDrums);
      Controls.Add(buttonTrim);
      Controls.Add(buttonSynth);
      Controls.Add(btnPlay);
      Controls.Add(buttonGuitar);
      Controls.Add(trkDrawTimescale);
      Controls.Add(lblDebug);
      Controls.Add(panTracks);
      Controls.Add(menu);
      KeyPreview = true;
      MainMenuStrip = menu;
      Margin = new Padding(3, 4, 3, 4);
      Name = "FrmMain";
      Text = "Mediocre Sound Designer";
      SizeChanged += FrmMain_SizeChanged;
      panTracks.ResumeLayout(false);
      menu.ResumeLayout(false);
      menu.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel panTracks;
    private Label lblDebug;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menu;
    private ToolStripMenuItem menuEdit;
    private Button btnPlay;
    private ToolStripMenuItem menuEditVolume;
    private ToolStripMenuItem menuEditReverse;
    private ToolStripMenuItem menuFile;
    private ToolStripMenuItem menuFileOpen;
    private ToolStripMenuItem menuFileSave;
    private System.Windows.Forms.Timer tmrUpdate;
    private ToolStripMenuItem menuGen;
    private ToolStripMenuItem menuGenNoise;
    private ToolStripMenuItem menuGenGradientNoise;
    private ToolStripMenuItem menuEditFlip;
    private SaveFileDialog sfdSaveFile;
    private TrackBar trkDrawTimescale;
    private ColorDialog colorDialog;
    private ToolStripMenuItem menuEditEliminateSilence;
    private Label lblPlayhead;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem saveAsToolStripMenuItem;
    private Button buttonRecord;
    private Button buttonTrim;
    private Button buttonGuitar;
    private Button buttonSynth;
    private Button buttonDrums;
    private Button buttonPiano;
    private TextBox lbGeneratorPanel;
    private ImageList imageList1;
    private Button buttonReset;
    private Button buttonDeleteSelection;
    private Button buttonImport;
  }
}


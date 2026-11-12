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
            panTopActions = new Panel();
            bttnMicOptions = new Button();
            bttnRecord = new Button();
            bttnPlay = new Button();
            bttnReset = new Button();
            picVolume = new PictureBox();
            trkVolume = new TrackBar();
            lblVolumePercent = new Label();
            lblDebug = new Label();
            ofdLoadFile = new OpenFileDialog();
            menu = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuFileOpen = new ToolStripMenuItem();
            menuFileSave = new ToolStripMenuItem();
            menuFileSettings = new ToolStripMenuItem();
            menuEdit = new ToolStripMenuItem();
            menuEditFlip = new ToolStripMenuItem();
            menuEditReverse = new ToolStripMenuItem();
            menuEditEliminateSilence = new ToolStripMenuItem();
            menuGen = new ToolStripMenuItem();
            menuGenRecord = new ToolStripMenuItem();
            menuGenNoise = new ToolStripMenuItem();
            menuGenGradientNoise = new ToolStripMenuItem();
            tmrUpdate = new System.Windows.Forms.Timer(components);
            sfdSaveFile = new SaveFileDialog();
            colorDialog = new ColorDialog();
            panBottomActions = new Panel();
            bttnGenerate = new Button();
            picSoundWave = new PictureBox();
            lblCurrTime = new Label();
            trkDrawTimescale = new TrackBar();
            panApp = new Panel();
            panTracks.SuspendLayout();
            panTopActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkVolume).BeginInit();
            menu.SuspendLayout();
            panBottomActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSoundWave).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).BeginInit();
            panApp.SuspendLayout();
            SuspendLayout();
            // 
            // panTracks
            // 
            panTracks.AutoScroll = true;
            panTracks.BackColor = Color.DimGray;
            panTracks.BorderStyle = BorderStyle.Fixed3D;
            panTracks.Controls.Add(lblPlayhead);
            panTracks.Location = new Point(0, 49);
            panTracks.Name = "panTracks";
            panTracks.Size = new Size(1598, 617);
            panTracks.TabIndex = 0;
            panTracks.Paint += panTracks_Paint;
            // 
            // lblPlayhead
            // 
            lblPlayhead.BackColor = Color.White;
            lblPlayhead.Location = new Point(154, 1);
            lblPlayhead.Name = "lblPlayhead";
            lblPlayhead.Size = new Size(5, 385);
            lblPlayhead.TabIndex = 9;
            // 
            // panTopActions
            // 
            panTopActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panTopActions.BackColor = Color.DarkGray;
            panTopActions.Controls.Add(bttnMicOptions);
            panTopActions.Controls.Add(bttnRecord);
            panTopActions.Controls.Add(bttnPlay);
            panTopActions.Controls.Add(bttnReset);
            panTopActions.Controls.Add(picVolume);
            panTopActions.Controls.Add(trkVolume);
            panTopActions.Controls.Add(lblVolumePercent);
            panTopActions.ForeColor = Color.White;
            panTopActions.Location = new Point(2, 27);
            panTopActions.Name = "panTopActions";
            panTopActions.Size = new Size(1604, 49);
            panTopActions.TabIndex = 10;
            // 
            // bttnMicOptions
            // 
            bttnMicOptions.AutoSize = true;
            bttnMicOptions.BackColor = Color.Transparent;
            bttnMicOptions.BackgroundImage = (Image)resources.GetObject("bttnMicOptions.BackgroundImage");
            bttnMicOptions.BackgroundImageLayout = ImageLayout.Stretch;
            bttnMicOptions.FlatAppearance.BorderSize = 0;
            bttnMicOptions.FlatAppearance.MouseDownBackColor = Color.Transparent;
            bttnMicOptions.FlatAppearance.MouseOverBackColor = Color.Transparent;
            bttnMicOptions.FlatStyle = FlatStyle.Flat;
            bttnMicOptions.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bttnMicOptions.Location = new Point(190, 5);
            bttnMicOptions.Name = "bttnMicOptions";
            bttnMicOptions.Size = new Size(38, 38);
            bttnMicOptions.TabIndex = 8;
            bttnMicOptions.UseVisualStyleBackColor = false;
            bttnMicOptions.Click += menuIOAudioMedia_Click;
            // 
            // bttnRecord
            // 
            bttnRecord.AutoSize = true;
            bttnRecord.BackColor = Color.Transparent;
            bttnRecord.BackgroundImage = (Image)resources.GetObject("bttnRecord.BackgroundImage");
            bttnRecord.BackgroundImageLayout = ImageLayout.Stretch;
            bttnRecord.FlatAppearance.BorderSize = 0;
            bttnRecord.FlatAppearance.MouseDownBackColor = Color.Transparent;
            bttnRecord.FlatAppearance.MouseOverBackColor = Color.Transparent;
            bttnRecord.FlatStyle = FlatStyle.Flat;
            bttnRecord.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bttnRecord.Location = new Point(130, 5);
            bttnRecord.Name = "bttnRecord";
            bttnRecord.Size = new Size(41, 41);
            bttnRecord.TabIndex = 7;
            bttnRecord.UseVisualStyleBackColor = false;
            bttnRecord.Click += menuGenRecord_Click;
            // 
            // bttnPlay
            // 
            bttnPlay.AutoSize = true;
            bttnPlay.BackColor = Color.Transparent;
            bttnPlay.BackgroundImage = (Image)resources.GetObject("bttnPlay.BackgroundImage");
            bttnPlay.BackgroundImageLayout = ImageLayout.Stretch;
            bttnPlay.FlatAppearance.BorderSize = 0;
            bttnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent;
            bttnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent;
            bttnPlay.FlatStyle = FlatStyle.Flat;
            bttnPlay.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bttnPlay.Location = new Point(70, 4);
            bttnPlay.Name = "bttnPlay";
            bttnPlay.Size = new Size(41, 41);
            bttnPlay.TabIndex = 6;
            bttnPlay.UseVisualStyleBackColor = false;
            bttnPlay.Click += btnPlay_Click;
            // 
            // bttnReset
            // 
            bttnReset.AutoSize = true;
            bttnReset.BackColor = Color.Transparent;
            bttnReset.BackgroundImageLayout = ImageLayout.Stretch;
            bttnReset.FlatAppearance.BorderSize = 0;
            bttnReset.FlatAppearance.MouseDownBackColor = Color.Transparent;
            bttnReset.FlatAppearance.MouseOverBackColor = Color.Transparent;
            bttnReset.FlatStyle = FlatStyle.Flat;
            bttnReset.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bttnReset.Location = new Point(10, 4);
            bttnReset.Name = "bttnReset";
            bttnReset.Size = new Size(41, 41);
            bttnReset.TabIndex = 11;
            bttnReset.TabStop = false;
            bttnReset.UseVisualStyleBackColor = false;
            bttnReset.Click += bttnReset_Click;
            // 
            // picVolume
            // 
            picVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picVolume.BackColor = Color.Transparent;
            picVolume.BackgroundImage = Properties.Resources.volume;
            picVolume.BackgroundImageLayout = ImageLayout.Stretch;
            picVolume.Location = new Point(1360, 7);
            picVolume.Name = "picVolume";
            picVolume.Size = new Size(34, 34);
            picVolume.TabIndex = 11;
            picVolume.TabStop = false;
            // 
            // trkVolume
            // 
            trkVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trkVolume.AutoSize = false;
            trkVolume.Location = new Point(1400, 7);
            trkVolume.Maximum = 200;
            trkVolume.Name = "trkVolume";
            trkVolume.Size = new Size(149, 35);
            trkVolume.TabIndex = 12;
            trkVolume.TickFrequency = 25;
            trkVolume.Value = 100;
            trkVolume.Scroll += trkVolume_Scroll;
            // 
            // lblVolumePercent
            // 
            lblVolumePercent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblVolumePercent.AutoSize = true;
            lblVolumePercent.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblVolumePercent.ForeColor = Color.Black;
            lblVolumePercent.Location = new Point(1550, 14);
            lblVolumePercent.Name = "lblVolumePercent";
            lblVolumePercent.Size = new Size(35, 15);
            lblVolumePercent.TabIndex = 13;
            lblVolumePercent.Text = "100%";
            // 
            // lblDebug
            // 
            lblDebug.Dock = DockStyle.Bottom;
            lblDebug.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDebug.ForeColor = Color.FromArgb(255, 255, 192);
            lblDebug.Location = new Point(0, 773);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(1604, 10);
            lblDebug.TabIndex = 1;
            // 
            // ofdLoadFile
            // 
            ofdLoadFile.FileName = "openFileDialog1";
            // 
            // menu
            // 
            menu.Items.AddRange(new ToolStripItem[] { menuFile, menuEdit, menuGen });
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Size = new Size(1604, 24);
            menu.TabIndex = 5;
            menu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuFileSave, menuFileSettings });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(37, 20);
            menuFile.Text = "&File";
            // 
            // menuFileOpen
            // 
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.Size = new Size(125, 22);
            menuFileOpen.Text = "Open...";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuFileSave
            // 
            menuFileSave.Name = "menuFileSave";
            menuFileSave.Size = new Size(125, 22);
            menuFileSave.Text = "Save...";
            menuFileSave.Click += menuFileSave_Click;
            // 
            // menuFileSettings
            // 
            menuFileSettings.Name = "menuFileSettings";
            menuFileSettings.Size = new Size(125, 22);
            menuFileSettings.Text = "Settings...";
            menuFileSettings.Click += menuFileSettings_Click;
            // 
            // menuEdit
            // 
            menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditFlip, menuEditReverse, menuEditEliminateSilence });
            menuEdit.Name = "menuEdit";
            menuEdit.Size = new Size(39, 20);
            menuEdit.Text = "E&dit";
            // 
            // menuEditFlip
            // 
            menuEditFlip.Name = "menuEditFlip";
            menuEditFlip.Size = new Size(172, 22);
            menuEditFlip.Text = "Flip";
            menuEditFlip.Click += menuEditFlip_Click;
            // 
            // menuEditReverse
            // 
            menuEditReverse.Name = "menuEditReverse";
            menuEditReverse.Size = new Size(172, 22);
            menuEditReverse.Text = "Reverse";
            menuEditReverse.Click += menuEditReverse_Click;
            // 
            // menuEditEliminateSilence
            // 
            menuEditEliminateSilence.Name = "menuEditEliminateSilence";
            menuEditEliminateSilence.Size = new Size(172, 22);
            menuEditEliminateSilence.Text = "Eliminate Silence...";
            menuEditEliminateSilence.Click += menuEditEliminateSilence_Click;
            // 
            // menuGen
            // 
            menuGen.DropDownItems.AddRange(new ToolStripItem[] { menuGenRecord, menuGenNoise, menuGenGradientNoise });
            menuGen.Name = "menuGen";
            menuGen.Size = new Size(66, 20);
            menuGen.Text = "&Generate";
            // 
            // menuGenRecord
            // 
            menuGenRecord.Name = "menuGenRecord";
            menuGenRecord.Size = new Size(152, 22);
            menuGenRecord.Text = "Record...";
            menuGenRecord.Click += menuGenRecord_Click;
            // 
            // menuGenNoise
            // 
            menuGenNoise.Name = "menuGenNoise";
            menuGenNoise.Size = new Size(152, 22);
            menuGenNoise.Text = "Noise";
            menuGenNoise.Click += menuGenNoise_Click;
            // 
            // menuGenGradientNoise
            // 
            menuGenGradientNoise.Name = "menuGenGradientNoise";
            menuGenGradientNoise.Size = new Size(152, 22);
            menuGenGradientNoise.Text = "Gradient Noise";
            menuGenGradientNoise.Click += menuGenGradientNoise_Click;
            // 
            // tmrUpdate
            // 
            tmrUpdate.Enabled = true;
            tmrUpdate.Interval = 20;
            tmrUpdate.Tick += tmrUpdatePlaybackState_Tick;
            // 
            // panBottomActions
            // 
            panBottomActions.BackColor = Color.DarkGray;
            panBottomActions.Controls.Add(bttnGenerate);
            panBottomActions.Controls.Add(picSoundWave);
            panBottomActions.Controls.Add(lblCurrTime);
            panBottomActions.Controls.Add(trkDrawTimescale);
            panBottomActions.ForeColor = Color.White;
            panBottomActions.Location = new Point(0, 666);
            panBottomActions.Name = "panBottomActions";
            panBottomActions.Size = new Size(1604, 65);
            panBottomActions.TabIndex = 8;
            // 
            // bttnGenerate
            // 
            bttnGenerate.BackColor = Color.DarkGray;
            bttnGenerate.Cursor = Cursors.Hand;
            bttnGenerate.FlatAppearance.BorderSize = 0;
            bttnGenerate.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            bttnGenerate.FlatAppearance.MouseOverBackColor = Color.LightGray;
            bttnGenerate.FlatStyle = FlatStyle.Flat;
            bttnGenerate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            bttnGenerate.ForeColor = Color.Black;
            bttnGenerate.Image = (Image)resources.GetObject("bttnGenerate.Image");
            bttnGenerate.ImageAlign = ContentAlignment.MiddleLeft;
            bttnGenerate.Location = new Point(12, 6);
            bttnGenerate.Name = "bttnGenerate";
            bttnGenerate.Padding = new Padding(10, 0, 0, 0);
            bttnGenerate.Size = new Size(190, 50);
            bttnGenerate.TabIndex = 1;
            bttnGenerate.Text = "Generate Sound";
            bttnGenerate.TextImageRelation = TextImageRelation.ImageBeforeText;
            bttnGenerate.UseVisualStyleBackColor = false;
            bttnGenerate.Click += bttnGenerate_Click;
            // 
            // picSoundWave
            // 
            picSoundWave.BackgroundImage = Properties.Resources.soundWavesIcon;
            picSoundWave.BackgroundImageLayout = ImageLayout.Stretch;
            picSoundWave.Location = new Point(1064, 6);
            picSoundWave.Name = "picSoundWave";
            picSoundWave.Size = new Size(50, 50);
            picSoundWave.TabIndex = 10;
            picSoundWave.TabStop = false;
            // 
            // lblCurrTime
            // 
            lblCurrTime.AutoSize = true;
            lblCurrTime.BackColor = Color.Transparent;
            lblCurrTime.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCurrTime.ForeColor = Color.Black;
            lblCurrTime.Location = new Point(634, 15);
            lblCurrTime.Name = "lblCurrTime";
            lblCurrTime.Size = new Size(249, 37);
            lblCurrTime.TabIndex = 9;
            lblCurrTime.Text = "00:00.00/00:00.00";
            // 
            // trkDrawTimescale
            // 
            trkDrawTimescale.AutoSize = false;
            trkDrawTimescale.LargeChange = 50;
            trkDrawTimescale.Location = new Point(1120, 15);
            trkDrawTimescale.Maximum = 1000;
            trkDrawTimescale.Minimum = 1;
            trkDrawTimescale.Name = "trkDrawTimescale";
            trkDrawTimescale.Size = new Size(470, 45);
            trkDrawTimescale.SmallChange = 10;
            trkDrawTimescale.TabIndex = 8;
            trkDrawTimescale.TickFrequency = 40;
            trkDrawTimescale.Value = 500;
            trkDrawTimescale.Scroll += trkDrawTimescale_Scroll;
            // 
            // panApp
            // 
            panApp.Controls.Add(panTopActions);
            panApp.Controls.Add(panBottomActions);
            panApp.Controls.Add(panTracks);
            panApp.Dock = DockStyle.Fill;
            panApp.Location = new Point(0, 0);
            panApp.Name = "panApp";
            panApp.Size = new Size(1604, 783);
            panApp.TabIndex = 11;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1604, 783);
            Controls.Add(lblDebug);
            Controls.Add(menu);
            Controls.Add(panApp);
            KeyPreview = true;
            MainMenuStrip = menu;
            Name = "FrmMain";
            Text = "Mediocre Sound Designer";
            Load += FrmMain_Load;
            SizeChanged += FrmMain_SizeChanged;
            panTracks.ResumeLayout(false);
            panTopActions.ResumeLayout(false);
            panTopActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkVolume).EndInit();
            menu.ResumeLayout(false);
            menu.PerformLayout();
            panBottomActions.ResumeLayout(false);
            panBottomActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picSoundWave).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).EndInit();
            panApp.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panTracks;
    private Label lblDebug;
    private OpenFileDialog ofdLoadFile;
    private MenuStrip menu;
    private ToolStripMenuItem menuEdit;
    private Button bttnPlay;
    private Button bttnReset;
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
    private TrackBar trkVolume;
    private Label lblCurrTime;
    private Label lblVolumePercent;
    private Panel panTopActions;
    private ColorDialog colorDialog;
    private ToolStripMenuItem menuEditEliminateSilence;
    private Label lblPlayhead;
    private ToolStripMenuItem menuGenRecord;
    private ToolStripMenuItem menuFileSettings;
    private Panel panBottomActions;
    private TextBox textBox1;
    private Button bttnRecord;
    private Button bttnMicOptions;
    private Panel panApp;
    private PictureBox picSoundWave;
    private PictureBox picVolume;
        private Button bttnGenerate;
    }
}
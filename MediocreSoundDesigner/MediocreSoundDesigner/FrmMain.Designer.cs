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
            menuEditVolume = new ToolStripMenuItem();
            menuEditEliminateSilence = new ToolStripMenuItem();
            menuGen = new ToolStripMenuItem();
            menuGenRecord = new ToolStripMenuItem();
            menuGenNoise = new ToolStripMenuItem();
            menuGenGradientNoise = new ToolStripMenuItem();
            bttnPlay = new Button();
            tmrUpdate = new System.Windows.Forms.Timer(components);
            sfdSaveFile = new SaveFileDialog();
            colorDialog = new ColorDialog();
            BottomPanel = new Panel();
            lblCurrTime = new Label();
            trkDrawTimescale = new TrackBar();
            pnlTopActions = new Panel();
            bttnMicOptions = new Button();
            bttnRecord = new Button();
            panTracks.SuspendLayout();
            menu.SuspendLayout();
            BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).BeginInit();
            pnlTopActions.SuspendLayout();
            SuspendLayout();
            // 
            // panTracks
            // 
            panTracks.AutoScroll = true;
            panTracks.BackColor = Color.DimGray;
            panTracks.BorderStyle = BorderStyle.Fixed3D;
            panTracks.Controls.Add(lblPlayhead);
            panTracks.Location = new Point(0, 73);
            panTracks.Name = "panTracks";
            panTracks.Size = new Size(1612, 647);
            panTracks.TabIndex = 0;
            // 
            // lblPlayhead
            // 
            lblPlayhead.BackColor = Color.White;
            lblPlayhead.Location = new Point(154, 1);
            lblPlayhead.Name = "lblPlayhead";
            lblPlayhead.Size = new Size(5, 385);
            lblPlayhead.TabIndex = 9;
            // 
            // lblDebug
            // 
            lblDebug.Dock = DockStyle.Bottom;
            lblDebug.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDebug.ForeColor = Color.FromArgb(255, 255, 192);
            lblDebug.Location = new Point(0, 746);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(1604, 37);
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
            menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditFlip, menuEditReverse, menuEditVolume, menuEditEliminateSilence });
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
            // menuEditVolume
            // 
            menuEditVolume.Name = "menuEditVolume";
            menuEditVolume.Size = new Size(172, 22);
            menuEditVolume.Text = "Alter Volume...";
            menuEditVolume.Click += menuEditVolume_Click;
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
            // tmrUpdate
            // 
            tmrUpdate.Enabled = true;
            tmrUpdate.Interval = 20;
            tmrUpdate.Tick += tmrUpdatePlaybackState_Tick;
            // 
            // BottomPanel
            // 
            BottomPanel.BackColor = Color.DarkGray;
            BottomPanel.Controls.Add(lblCurrTime);
            BottomPanel.Controls.Add(trkDrawTimescale);
            BottomPanel.ForeColor = Color.White;
            BottomPanel.Location = new Point(0, 718);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Size(1604, 65);
            BottomPanel.TabIndex = 8;
            // 
            // lblCurrTime
            // 
            lblCurrTime.AutoSize = true;
            lblCurrTime.BackColor = Color.Transparent;
            lblCurrTime.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCurrTime.ForeColor = Color.Black;
            lblCurrTime.Location = new Point(634, 15);
            lblCurrTime.Name = "lblCurrTime";
            lblCurrTime.Size = new Size(270, 37);
            lblCurrTime.TabIndex = 9;
            lblCurrTime.Text = "00:00.00  / 00:00.00";
            lblCurrTime.Visible = false;
            // 
            // trkDrawTimescale
            // 
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
            // pnlTopActions
            // 
            pnlTopActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlTopActions.BackColor = Color.DarkGray;
            pnlTopActions.Controls.Add(bttnMicOptions);
            pnlTopActions.Controls.Add(bttnRecord);
            pnlTopActions.Controls.Add(bttnPlay);
            pnlTopActions.ForeColor = Color.White;
            pnlTopActions.Location = new Point(0, 24);
            pnlTopActions.Name = "pnlTopActions";
            pnlTopActions.Size = new Size(1604, 49);
            pnlTopActions.TabIndex = 10;
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
            bttnMicOptions.Location = new Point(136, 5);
            bttnMicOptions.Name = "bttnMicOptions";
            bttnMicOptions.Size = new Size(38, 38);
            bttnMicOptions.TabIndex = 8;
            bttnMicOptions.UseVisualStyleBackColor = false;
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
            bttnRecord.Location = new Point(10, 5);
            bttnRecord.Name = "bttnRecord";
            bttnRecord.Size = new Size(41, 41);
            bttnRecord.TabIndex = 7;
            bttnRecord.UseVisualStyleBackColor = false;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1604, 783);
            Controls.Add(pnlTopActions);
            Controls.Add(BottomPanel);
            Controls.Add(lblDebug);
            Controls.Add(panTracks);
            Controls.Add(menu);
            KeyPreview = true;
            MainMenuStrip = menu;
            Name = "FrmMain";
            Text = "Mediocre Sound Designer";
            SizeChanged += FrmMain_SizeChanged;
            panTracks.ResumeLayout(false);
            menu.ResumeLayout(false);
            menu.PerformLayout();
            BottomPanel.ResumeLayout(false);
            BottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkDrawTimescale).EndInit();
            pnlTopActions.ResumeLayout(false);
            pnlTopActions.PerformLayout();
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
        private Label lblCurrTime;
        private Panel pnlTopActions;
        private ColorDialog colorDialog;
    private ToolStripMenuItem menuEditEliminateSilence;
    private Label lblPlayhead;
    private ToolStripMenuItem menuGenRecord;
    private ToolStripMenuItem menuFileSettings;
        private Panel BottomPanel;
        private TextBox textBox1;
        private Button bttnRecord;
        private Button bttnMicOptions;
    }
}

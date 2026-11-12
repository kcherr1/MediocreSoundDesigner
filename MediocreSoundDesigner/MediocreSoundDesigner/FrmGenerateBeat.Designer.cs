namespace MediocreSoundDesigner
{
  partial class FrmGenerateBeat
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      cmbRhythm = new ComboBox();
      lblRhythm = new Label();
      nudBars = new NumericUpDown();
      lblBars = new Label();
      nudStartTime = new NumericUpDown();
      lblStartTime = new Label();
      nudTempo = new NumericUpDown();
      lblTempo = new Label();
      cmbDrumKit = new ComboBox();
      lblDrumKit = new Label();
      btnCancel = new Button();
      btnCreate = new Button();
      ((System.ComponentModel.ISupportInitialize)nudBars).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudTempo).BeginInit();
      SuspendLayout();
      // 
      // cmbRhythm
      // 
      cmbRhythm.BackColor = Color.DimGray;
      cmbRhythm.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbRhythm.FlatStyle = FlatStyle.Flat;
      cmbRhythm.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      cmbRhythm.ForeColor = Color.White;
      cmbRhythm.FormattingEnabled = true;
      cmbRhythm.Items.AddRange(new object[] { "Rock", "Pop", "Hip-Hop", "Four on the Floor" });
      cmbRhythm.Location = new Point(189, 84);
      cmbRhythm.Name = "cmbRhythm";
      cmbRhythm.Size = new Size(270, 33);
      cmbRhythm.TabIndex = 45;
      // 
      // lblRhythm
      // 
      lblRhythm.BackColor = Color.Transparent;
      lblRhythm.Font = new Font("Segoe UI", 10.8F);
      lblRhythm.ForeColor = Color.FromArgb(255, 255, 192);
      lblRhythm.ImeMode = ImeMode.NoControl;
      lblRhythm.Location = new Point(24, 86);
      lblRhythm.Name = "lblRhythm";
      lblRhythm.Size = new Size(140, 25);
      lblRhythm.TabIndex = 44;
      lblRhythm.Text = "Rhythm";
      // 
      // nudBars
      // 
      nudBars.BackColor = Color.DimGray;
      nudBars.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudBars.ForeColor = Color.White;
      nudBars.Location = new Point(189, 204);
      nudBars.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
      nudBars.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      nudBars.Name = "nudBars";
      nudBars.Size = new Size(270, 31);
      nudBars.TabIndex = 43;
      nudBars.ThousandsSeparator = true;
      nudBars.Value = new decimal(new int[] { 4, 0, 0, 0 });
      // 
      // lblBars
      // 
      lblBars.BackColor = Color.Transparent;
      lblBars.Font = new Font("Segoe UI", 10.8F);
      lblBars.ForeColor = Color.FromArgb(255, 255, 192);
      lblBars.ImeMode = ImeMode.NoControl;
      lblBars.Location = new Point(24, 206);
      lblBars.Name = "lblBars";
      lblBars.Size = new Size(140, 25);
      lblBars.TabIndex = 42;
      lblBars.Text = "Bars";
      // 
      // nudStartTime
      // 
      nudStartTime.BackColor = Color.DimGray;
      nudStartTime.DecimalPlaces = 1;
      nudStartTime.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudStartTime.ForeColor = Color.White;
      nudStartTime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudStartTime.Location = new Point(189, 264);
      nudStartTime.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
      nudStartTime.Name = "nudStartTime";
      nudStartTime.Size = new Size(270, 31);
      nudStartTime.TabIndex = 39;
      nudStartTime.ThousandsSeparator = true;
      // 
      // lblStartTime
      // 
      lblStartTime.BackColor = Color.Transparent;
      lblStartTime.Font = new Font("Segoe UI", 10.8F);
      lblStartTime.ForeColor = Color.FromArgb(255, 255, 192);
      lblStartTime.ImeMode = ImeMode.NoControl;
      lblStartTime.Location = new Point(24, 266);
      lblStartTime.Name = "lblStartTime";
      lblStartTime.Size = new Size(155, 25);
      lblStartTime.TabIndex = 38;
      lblStartTime.Text = "Start Time (sec)";
      // 
      // nudTempo
      // 
      nudTempo.BackColor = Color.DimGray;
      nudTempo.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudTempo.ForeColor = Color.White;
      nudTempo.Location = new Point(189, 144);
      nudTempo.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
      nudTempo.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
      nudTempo.Name = "nudTempo";
      nudTempo.Size = new Size(270, 31);
      nudTempo.TabIndex = 37;
      nudTempo.ThousandsSeparator = true;
      nudTempo.Value = new decimal(new int[] { 120, 0, 0, 0 });
      // 
      // lblTempo
      // 
      lblTempo.BackColor = Color.Transparent;
      lblTempo.Font = new Font("Segoe UI", 10.8F);
      lblTempo.ForeColor = Color.FromArgb(255, 255, 192);
      lblTempo.ImeMode = ImeMode.NoControl;
      lblTempo.Location = new Point(24, 146);
      lblTempo.Name = "lblTempo";
      lblTempo.Size = new Size(140, 25);
      lblTempo.TabIndex = 36;
      lblTempo.Text = "Tempo (BPM)";
      // 
      // cmbDrumKit
      // 
      cmbDrumKit.BackColor = Color.DimGray;
      cmbDrumKit.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbDrumKit.FlatStyle = FlatStyle.Flat;
      cmbDrumKit.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      cmbDrumKit.ForeColor = Color.White;
      cmbDrumKit.FormattingEnabled = true;
      cmbDrumKit.Items.AddRange(new object[] { "Standard", "Room", "Electronic", "Jazz", "Brush", "SFX" });
      cmbDrumKit.Location = new Point(189, 24);
      cmbDrumKit.Name = "cmbDrumKit";
      cmbDrumKit.Size = new Size(270, 33);
      cmbDrumKit.TabIndex = 35;
      cmbDrumKit.SelectedIndexChanged += cmbDrumKit_SelectedIndexChanged;
      // 
      // lblDrumKit
      // 
      lblDrumKit.BackColor = Color.Transparent;
      lblDrumKit.Font = new Font("Segoe UI", 10.8F);
      lblDrumKit.ForeColor = Color.FromArgb(255, 255, 192);
      lblDrumKit.ImeMode = ImeMode.NoControl;
      lblDrumKit.Location = new Point(24, 26);
      lblDrumKit.Name = "lblDrumKit";
      lblDrumKit.Size = new Size(140, 25);
      lblDrumKit.TabIndex = 34;
      lblDrumKit.Text = "Drum Kit";
      // 
      // btnCancel
      // 
      btnCancel.BackColor = Color.FromArgb(255, 128, 128);
      btnCancel.DialogResult = DialogResult.Cancel;
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
      btnCancel.ForeColor = Color.Black;
      btnCancel.ImeMode = ImeMode.NoControl;
      btnCancel.Location = new Point(97, 334);
      btnCancel.Margin = new Padding(3, 2, 3, 2);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(90, 35);
      btnCancel.TabIndex = 47;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      // 
      // btnCreate
      // 
      btnCreate.BackColor = Color.FromArgb(128, 255, 128);
      btnCreate.DialogResult = DialogResult.OK;
      btnCreate.FlatStyle = FlatStyle.Flat;
      btnCreate.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
      btnCreate.ForeColor = Color.Black;
      btnCreate.ImeMode = ImeMode.NoControl;
      btnCreate.Location = new Point(307, 334);
      btnCreate.Margin = new Padding(3, 2, 3, 2);
      btnCreate.Name = "btnCreate";
      btnCreate.Size = new Size(90, 35);
      btnCreate.TabIndex = 46;
      btnCreate.Text = "Create";
      btnCreate.UseVisualStyleBackColor = false;
      // 
      // FrmGenerateBeat
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(482, 393);
      Controls.Add(btnCancel);
      Controls.Add(btnCreate);
      Controls.Add(cmbRhythm);
      Controls.Add(lblRhythm);
      Controls.Add(nudBars);
      Controls.Add(lblBars);
      Controls.Add(nudStartTime);
      Controls.Add(lblStartTime);
      Controls.Add(nudTempo);
      Controls.Add(lblTempo);
      Controls.Add(cmbDrumKit);
      Controls.Add(lblDrumKit);
      ForeColor = Color.White;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmGenerateBeat";
      ShowIcon = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Generate Beat";
      ((System.ComponentModel.ISupportInitialize)nudBars).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudTempo).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private ComboBox cmbRhythm;
    private Label lblRhythm;
    private NumericUpDown nudBars;
    private Label lblBars;
    private NumericUpDown nudStartTime;
    private Label lblStartTime;
    private NumericUpDown nudTempo;
    private Label lblTempo;
    private ComboBox cmbDrumKit;
    private Label lblDrumKit;
    private Button btnCancel;
    private Button btnCreate;
  }
}
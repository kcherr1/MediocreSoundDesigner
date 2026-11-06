namespace MediocreSoundDesigner
{
  partial class FrmGenerateSynth
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
      lblNote = new Label();
      nudStartTime = new NumericUpDown();
      lblStartTime = new Label();
      btnCancel = new Button();
      btnCreate = new Button();
      nudDuration = new NumericUpDown();
      lblDuration = new Label();
      nudOctave = new NumericUpDown();
      lblFrequency = new Label();
      cmbInstrument = new ComboBox();
      lblInstrument = new Label();
      cmbNote = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudOctave).BeginInit();
      SuspendLayout();
      // 
      // lblNote
      // 
      lblNote.BackColor = Color.Transparent;
      lblNote.Font = new Font("Segoe UI", 10.8F);
      lblNote.ImeMode = ImeMode.NoControl;
      lblNote.Location = new Point(49, 86);
      lblNote.Name = "lblNote";
      lblNote.Size = new Size(140, 25);
      lblNote.TabIndex = 32;
      lblNote.Text = "Note";
      // 
      // nudStartTime
      // 
      nudStartTime.BackColor = Color.DimGray;
      nudStartTime.DecimalPlaces = 1;
      nudStartTime.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudStartTime.ForeColor = Color.White;
      nudStartTime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudStartTime.Location = new Point(214, 204);
      nudStartTime.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
      nudStartTime.Name = "nudStartTime";
      nudStartTime.Size = new Size(270, 31);
      nudStartTime.TabIndex = 31;
      nudStartTime.ThousandsSeparator = true;
      // 
      // lblStartTime
      // 
      lblStartTime.BackColor = Color.Transparent;
      lblStartTime.Font = new Font("Segoe UI", 10.8F);
      lblStartTime.ImeMode = ImeMode.NoControl;
      lblStartTime.Location = new Point(49, 206);
      lblStartTime.Name = "lblStartTime";
      lblStartTime.Size = new Size(140, 25);
      lblStartTime.TabIndex = 30;
      lblStartTime.Text = "Start Time (sec)";
      // 
      // btnCancel
      // 
      btnCancel.BackColor = Color.DimGray;
      btnCancel.DialogResult = DialogResult.Cancel;
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI", 10.2F);
      btnCancel.ImeMode = ImeMode.NoControl;
      btnCancel.Location = new Point(335, 334);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(90, 35);
      btnCancel.TabIndex = 29;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      // 
      // btnCreate
      // 
      btnCreate.BackColor = Color.DimGray;
      btnCreate.DialogResult = DialogResult.OK;
      btnCreate.FlatStyle = FlatStyle.Flat;
      btnCreate.Font = new Font("Segoe UI", 10.2F);
      btnCreate.ImeMode = ImeMode.NoControl;
      btnCreate.Location = new Point(125, 334);
      btnCreate.Name = "btnCreate";
      btnCreate.Size = new Size(90, 35);
      btnCreate.TabIndex = 28;
      btnCreate.Text = "Create";
      btnCreate.UseVisualStyleBackColor = false;
      btnCreate.Click += btnCreate_Click;
      // 
      // nudDuration
      // 
      nudDuration.BackColor = Color.DimGray;
      nudDuration.DecimalPlaces = 1;
      nudDuration.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudDuration.ForeColor = Color.White;
      nudDuration.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Location = new Point(214, 264);
      nudDuration.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
      nudDuration.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Name = "nudDuration";
      nudDuration.Size = new Size(270, 31);
      nudDuration.TabIndex = 27;
      nudDuration.ThousandsSeparator = true;
      nudDuration.Value = new decimal(new int[] { 2, 0, 0, 0 });
      // 
      // lblDuration
      // 
      lblDuration.BackColor = Color.Transparent;
      lblDuration.Font = new Font("Segoe UI", 10.8F);
      lblDuration.ImeMode = ImeMode.NoControl;
      lblDuration.Location = new Point(49, 266);
      lblDuration.Name = "lblDuration";
      lblDuration.Size = new Size(155, 25);
      lblDuration.TabIndex = 26;
      lblDuration.Text = "Duration (sec)";
      // 
      // nudOctave
      // 
      nudOctave.BackColor = Color.DimGray;
      nudOctave.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      nudOctave.ForeColor = Color.White;
      nudOctave.Location = new Point(214, 144);
      nudOctave.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
      nudOctave.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      nudOctave.Name = "nudOctave";
      nudOctave.Size = new Size(270, 31);
      nudOctave.TabIndex = 25;
      nudOctave.ThousandsSeparator = true;
      nudOctave.Value = new decimal(new int[] { 4, 0, 0, 0 });
      // 
      // lblFrequency
      // 
      lblFrequency.BackColor = Color.Transparent;
      lblFrequency.Font = new Font("Segoe UI", 10.8F);
      lblFrequency.ImeMode = ImeMode.NoControl;
      lblFrequency.Location = new Point(49, 146);
      lblFrequency.Name = "lblFrequency";
      lblFrequency.Size = new Size(140, 25);
      lblFrequency.TabIndex = 24;
      lblFrequency.Text = "Octave";
      // 
      // cmbInstrument
      // 
      cmbInstrument.BackColor = Color.DimGray;
      cmbInstrument.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbInstrument.FlatStyle = FlatStyle.Flat;
      cmbInstrument.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      cmbInstrument.ForeColor = Color.White;
      cmbInstrument.FormattingEnabled = true;
      cmbInstrument.Items.AddRange(new object[] { "Synth Lead 1", "Synth Lead 2", "Warm Pad", "Polysynth Pad" });
      cmbInstrument.Location = new Point(214, 24);
      cmbInstrument.Name = "cmbInstrument";
      cmbInstrument.Size = new Size(270, 33);
      cmbInstrument.TabIndex = 23;
      // 
      // lblInstrument
      // 
      lblInstrument.BackColor = Color.Transparent;
      lblInstrument.Font = new Font("Segoe UI", 10.8F);
      lblInstrument.ImeMode = ImeMode.NoControl;
      lblInstrument.Location = new Point(49, 26);
      lblInstrument.Name = "lblInstrument";
      lblInstrument.Size = new Size(140, 25);
      lblInstrument.TabIndex = 22;
      lblInstrument.Text = "Instrument";
      lblInstrument.Click += lblRootnote_Click;
      // 
      // cmbNote
      // 
      cmbNote.BackColor = Color.DimGray;
      cmbNote.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbNote.FlatStyle = FlatStyle.Flat;
      cmbNote.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      cmbNote.ForeColor = Color.White;
      cmbNote.FormattingEnabled = true;
      cmbNote.Items.AddRange(new object[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" });
      cmbNote.Location = new Point(214, 84);
      cmbNote.Name = "cmbNote";
      cmbNote.Size = new Size(270, 33);
      cmbNote.TabIndex = 33;
      // 
      // FrmGenerateSynth
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(532, 393);
      Controls.Add(cmbNote);
      Controls.Add(lblNote);
      Controls.Add(nudStartTime);
      Controls.Add(lblStartTime);
      Controls.Add(btnCancel);
      Controls.Add(btnCreate);
      Controls.Add(nudDuration);
      Controls.Add(lblDuration);
      Controls.Add(nudOctave);
      Controls.Add(lblFrequency);
      Controls.Add(cmbInstrument);
      Controls.Add(lblInstrument);
      ForeColor = Color.White;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmGenerateSynth";
      StartPosition = FormStartPosition.CenterParent;
      Text = "Generate Synth";
      ((System.ComponentModel.ISupportInitialize)nudStartTime).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudOctave).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Label lblNote;
    private NumericUpDown nudStartTime;
    private Label lblStartTime;
    private Button btnCancel;
    private Button btnCreate;
    private NumericUpDown nudDuration;
    private Label lblDuration;
    private NumericUpDown nudOctave;
    private Label lblFrequency;
    private ComboBox cmbInstrument;
    private Label lblInstrument;
    private ComboBox cmbNote;
  }
}
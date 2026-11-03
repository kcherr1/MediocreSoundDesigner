namespace MediocreSoundDesigner
{
  partial class GenerateTone
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
    /// Required method for Designer support - do not modify the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      lblWaveform = new Label();
      cmbWaveform = new ComboBox();
      lblFrequency = new Label();
      numericUpDown1 = new NumericUpDown();
      lblDuration = new Label();
      nudDuration = new NumericUpDown();
      btnCreate = new Button();
      btnCancel = new Button();
      nudStartTime = new NumericUpDown();
      lblStartTime = new Label();
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).BeginInit();
      SuspendLayout();
      // 
      // lblWaveform
      // 
      lblWaveform.BackColor = Color.Transparent;
      lblWaveform.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblWaveform.Location = new Point(24, 29);
      lblWaveform.Name = "lblWaveform";
      lblWaveform.Size = new Size(140, 25);
      lblWaveform.TabIndex = 0;
      lblWaveform.Text = "Waveform";
      lblWaveform.Click += lblWaveform_Click;
      // 
      // cmbWaveform
      // 
      cmbWaveform.BackColor = Color.DimGray;
      cmbWaveform.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbWaveform.FlatStyle = FlatStyle.Flat;
      cmbWaveform.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
      cmbWaveform.ForeColor = Color.White;
      cmbWaveform.FormattingEnabled = true;
      cmbWaveform.Items.AddRange(new object[] { "Sine", "Square", "Triangle", "Sawtooth" });
      cmbWaveform.Location = new Point(189, 26);
      cmbWaveform.Name = "cmbWaveform";
      cmbWaveform.Size = new Size(270, 33);
      cmbWaveform.TabIndex = 1;
      cmbWaveform.SelectedIndexChanged += cmbWaveform_SelectedIndexChanged;
      // 
      // lblFrequency
      // 
      lblFrequency.BackColor = Color.Transparent;
      lblFrequency.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblFrequency.Location = new Point(24, 89);
      lblFrequency.Name = "lblFrequency";
      lblFrequency.Size = new Size(140, 25);
      lblFrequency.TabIndex = 2;
      lblFrequency.Text = "Frequency (Hz)";
      lblFrequency.Click += lblFrequency_Click;
      // 
      // numericUpDown1
      // 
      numericUpDown1.BackColor = Color.DimGray;
      numericUpDown1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
      numericUpDown1.ForeColor = Color.White;
      numericUpDown1.Location = new Point(189, 87);
      numericUpDown1.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
      numericUpDown1.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
      numericUpDown1.Name = "numericUpDown1";
      numericUpDown1.Size = new Size(270, 31);
      numericUpDown1.TabIndex = 3;
      numericUpDown1.ThousandsSeparator = true;
      numericUpDown1.Value = new decimal(new int[] { 440, 0, 0, 0 });
      numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
      // 
      // lblDuration
      // 
      lblDuration.BackColor = Color.Transparent;
      lblDuration.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblDuration.Location = new Point(24, 209);
      lblDuration.Name = "lblDuration";
      lblDuration.Size = new Size(155, 25);
      lblDuration.TabIndex = 4;
      lblDuration.Text = "Duration (sec)";
      lblDuration.Click += lblDuration_Click;
      // 
      // nudDuration
      // 
      nudDuration.BackColor = Color.DimGray;
      nudDuration.DecimalPlaces = 1;
      nudDuration.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
      nudDuration.ForeColor = Color.White;
      nudDuration.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Location = new Point(189, 207);
      nudDuration.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
      nudDuration.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Name = "nudDuration";
      nudDuration.Size = new Size(270, 31);
      nudDuration.TabIndex = 5;
      nudDuration.ThousandsSeparator = true;
      nudDuration.Value = new decimal(new int[] { 2, 0, 0, 0 });
      nudDuration.ValueChanged += nudDuration_ValueChanged;
      // 
      // btnCreate
      // 
      btnCreate.BackColor = Color.DimGray;
      btnCreate.DialogResult = DialogResult.OK;
      btnCreate.FlatStyle = FlatStyle.Flat;
      btnCreate.Font = new Font("Segoe UI", 10.2F);
      btnCreate.Location = new Point(98, 280);
      btnCreate.Name = "btnCreate";
      btnCreate.Size = new Size(90, 35);
      btnCreate.TabIndex = 6;
      btnCreate.Text = "Create";
      btnCreate.UseVisualStyleBackColor = false;
      btnCreate.Click += btnCreate_Click;
      // 
      // btnCancel
      // 
      btnCancel.BackColor = Color.DimGray;
      btnCancel.DialogResult = DialogResult.Cancel;
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI", 10.2F);
      btnCancel.Location = new Point(310, 280);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(90, 35);
      btnCancel.TabIndex = 7;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += btnCancel_Click;
      // 
      // nudStartTime
      // 
      nudStartTime.BackColor = Color.DimGray;
      nudStartTime.DecimalPlaces = 1;
      nudStartTime.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
      nudStartTime.ForeColor = Color.White;
      nudStartTime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudStartTime.Location = new Point(189, 147);
      nudStartTime.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
      nudStartTime.Name = "nudStartTime";
      nudStartTime.Size = new Size(270, 31);
      nudStartTime.TabIndex = 9;
      nudStartTime.ThousandsSeparator = true;
      nudStartTime.ValueChanged += numericUpDown2_ValueChanged;
      // 
      // lblStartTime
      // 
      lblStartTime.BackColor = Color.Transparent;
      lblStartTime.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lblStartTime.Location = new Point(24, 149);
      lblStartTime.Name = "lblStartTime";
      lblStartTime.Size = new Size(140, 25);
      lblStartTime.TabIndex = 8;
      lblStartTime.Text = "Start Time (sec)";
      // 
      // GenerateTone
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(482, 343);
      Controls.Add(nudStartTime);
      Controls.Add(lblStartTime);
      Controls.Add(btnCancel);
      Controls.Add(btnCreate);
      Controls.Add(nudDuration);
      Controls.Add(lblDuration);
      Controls.Add(numericUpDown1);
      Controls.Add(lblFrequency);
      Controls.Add(cmbWaveform);
      Controls.Add(lblWaveform);
      ForeColor = Color.White;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "GenerateTone";
      ShowIcon = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Generate Tone";
      Load += GenerateTone_Load;
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Label lblWaveform;
    private ComboBox cmbWaveform;
    private Label lblFrequency;
    private NumericUpDown numericUpDown1;
    private Label lblDuration;
    private NumericUpDown nudDuration;
    private Button btnCreate;
    private Button btnCancel;
    private NumericUpDown nudStartTime;
    private Label lblStartTime;
  }
}
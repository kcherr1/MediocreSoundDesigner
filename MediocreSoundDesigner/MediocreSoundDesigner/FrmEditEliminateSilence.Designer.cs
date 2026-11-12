namespace MediocreSoundDesigner {
  partial class FrmEditEliminateSilence {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
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
      nudMinDuration = new NumericUpDown();
      label1 = new Label();
      label2 = new Label();
      nudThreshold = new NumericUpDown();
      label3 = new Label();
      btnCancel = new Button();
      btnApply = new Button();
      cmbWhere = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)nudMinDuration).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudThreshold).BeginInit();
      SuspendLayout();
      // 
      // nudMinDuration
      // 
      nudMinDuration.BackColor = Color.DimGray;
      nudMinDuration.DecimalPlaces = 2;
      nudMinDuration.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      nudMinDuration.ForeColor = Color.White;
      nudMinDuration.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      nudMinDuration.Location = new Point(314, 70);
      nudMinDuration.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
      nudMinDuration.Name = "nudMinDuration";
      nudMinDuration.Size = new Size(261, 31);
      nudMinDuration.TabIndex = 9;
      nudMinDuration.Value = new decimal(new int[] { 1, 0, 0, 65536 });
      nudMinDuration.ValueChanged += nudMinDuration_ValueChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 73);
      label1.Name = "label1";
      label1.Size = new Size(178, 28);
      label1.TabIndex = 10;
      label1.Text = "Minimum Duration";
      label1.Click += label1_Click;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(12, 128);
      label2.Name = "label2";
      label2.Size = new Size(98, 28);
      label2.TabIndex = 12;
      label2.Text = "Threshold";
      label2.Click += label2_Click;
      // 
      // nudThreshold
      // 
      nudThreshold.BackColor = Color.DimGray;
      nudThreshold.DecimalPlaces = 4;
      nudThreshold.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      nudThreshold.ForeColor = Color.White;
      nudThreshold.Increment = new decimal(new int[] { 25, 0, 0, 327680 });
      nudThreshold.Location = new Point(314, 129);
      nudThreshold.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
      nudThreshold.Name = "nudThreshold";
      nudThreshold.Size = new Size(261, 31);
      nudThreshold.TabIndex = 11;
      nudThreshold.Value = new decimal(new int[] { 1, 0, 0, 196608 });
      nudThreshold.ValueChanged += nudThreshold_ValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label3.Location = new Point(12, 19);
      label3.Name = "label3";
      label3.Size = new Size(253, 28);
      label3.TabIndex = 13;
      label3.Text = "Find Silent Sections Where?";
      // 
      // btnCancel
      // 
      btnCancel.AutoSize = true;
      btnCancel.BackColor = Color.FromArgb(255, 128, 128);
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnCancel.ForeColor = Color.Black;
      btnCancel.Location = new Point(95, 196);
      btnCancel.Margin = new Padding(7);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(95, 42);
      btnCancel.TabIndex = 16;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += btnCancel_Click;
      // 
      // btnApply
      // 
      btnApply.BackColor = Color.FromArgb(128, 255, 128);
      btnApply.FlatStyle = FlatStyle.Flat;
      btnApply.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnApply.ForeColor = Color.Black;
      btnApply.Location = new Point(378, 196);
      btnApply.Margin = new Padding(7);
      btnApply.Name = "btnApply";
      btnApply.Size = new Size(95, 42);
      btnApply.TabIndex = 15;
      btnApply.Text = "Apply";
      btnApply.UseVisualStyleBackColor = false;
      btnApply.Click += btnApply_Click;
      // 
      // cmbWhere
      // 
      cmbWhere.BackColor = Color.DimGray;
      cmbWhere.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbWhere.FlatStyle = FlatStyle.Flat;
      cmbWhere.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
      cmbWhere.ForeColor = Color.White;
      cmbWhere.FormattingEnabled = true;
      cmbWhere.Items.AddRange(new object[] { "Beginning", "End", "Beginning and End", "Anywhere" });
      cmbWhere.Location = new Point(314, 19);
      cmbWhere.Name = "cmbWhere";
      cmbWhere.Size = new Size(261, 33);
      cmbWhere.TabIndex = 17;
      // 
      // FrmEditEliminateSilence
      // 
      AutoScaleDimensions = new SizeF(11F, 28F);
      AutoScaleMode = AutoScaleMode.None;
      BackColor = Color.Black;
      ClientSize = new Size(604, 257);
      Controls.Add(cmbWhere);
      Controls.Add(btnCancel);
      Controls.Add(btnApply);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(nudThreshold);
      Controls.Add(label1);
      Controls.Add(nudMinDuration);
      Font = new Font("Segoe UI", 12F);
      ForeColor = Color.FromArgb(255, 255, 192);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmEditEliminateSilence";
      Text = "Eliminate Silence";
      ((System.ComponentModel.ISupportInitialize)nudMinDuration).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudThreshold).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private NumericUpDown nudMinDuration;
    private Label label1;
    private Label label2;
    private NumericUpDown nudThreshold;
    private Label label3;
    private Button btnCancel;
    private Button btnApply;
    private ComboBox cmbWhere;
  }
}
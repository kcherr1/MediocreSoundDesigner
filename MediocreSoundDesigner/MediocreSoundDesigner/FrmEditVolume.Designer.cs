namespace MediocreSoundDesigner {
  partial class FrmEditVolume {
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
      nudVolume = new NumericUpDown();
      label1 = new Label();
      btnApply = new Button();
      btnCancel = new Button();
      ((System.ComponentModel.ISupportInitialize)nudVolume).BeginInit();
      SuspendLayout();
      // 
      // nudVolume
      // 
      nudVolume.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      nudVolume.Increment = new decimal(new int[] { 2, 0, 0, 0 });
      nudVolume.Location = new Point(195, 58);
      nudVolume.Margin = new Padding(7);
      nudVolume.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
      nudVolume.Name = "nudVolume";
      nudVolume.Size = new Size(98, 34);
      nudVolume.TabIndex = 0;
      nudVolume.TextAlign = HorizontalAlignment.Center;
      nudVolume.Value = new decimal(new int[] { 100, 0, 0, 0 });
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
      label1.Location = new Point(56, 61);
      label1.Margin = new Padding(7, 0, 7, 0);
      label1.Name = "label1";
      label1.Size = new Size(99, 31);
      label1.TabIndex = 1;
      label1.Text = "Volume:";
      // 
      // btnApply
      // 
      btnApply.BackColor = Color.FromArgb(128, 255, 128);
      btnApply.FlatStyle = FlatStyle.Flat;
      btnApply.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnApply.ForeColor = Color.Black;
      btnApply.Location = new Point(236, 160);
      btnApply.Margin = new Padding(7);
      btnApply.Name = "btnApply";
      btnApply.Size = new Size(95, 42);
      btnApply.TabIndex = 2;
      btnApply.Text = "Apply";
      btnApply.UseVisualStyleBackColor = false;
      btnApply.Click += btnApply_Click;
      // 
      // btnCancel
      // 
      btnCancel.AutoSize = true;
      btnCancel.BackColor = Color.FromArgb(255, 128, 128);
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnCancel.ForeColor = Color.Black;
      btnCancel.Location = new Point(46, 160);
      btnCancel.Margin = new Padding(7);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(95, 42);
      btnCancel.TabIndex = 3;
      btnCancel.Text = "Cancel";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += btnCancel_Click;
      // 
      // FrmEditVolume
      // 
      AutoScaleDimensions = new SizeF(11F, 28F);
      AutoScaleMode = AutoScaleMode.None;
      BackColor = Color.Black;
      ClientSize = new Size(375, 225);
      Controls.Add(btnCancel);
      Controls.Add(btnApply);
      Controls.Add(label1);
      Controls.Add(nudVolume);
      Font = new Font("Segoe UI", 12F);
      ForeColor = Color.FromArgb(255, 255, 192);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      Margin = new Padding(16, 17, 16, 17);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmEditVolume";
      Text = "Edit Volume";
      ((System.ComponentModel.ISupportInitialize)nudVolume).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private NumericUpDown nudVolume;
    private Label label1;
    private Button btnApply;
    private Button btnCancel;
  }
}
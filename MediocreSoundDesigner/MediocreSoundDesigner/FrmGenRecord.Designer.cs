namespace MediocreSoundDesigner {
  partial class FrmGenRecord {
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
      btnRecord = new Button();
      btnCancel = new Button();
      btnApply = new Button();
      SuspendLayout();
      // 
      // btnRecord
      // 
      btnRecord.AutoSize = true;
      btnRecord.BackColor = Color.Black;
      btnRecord.BackgroundImage = Properties.Resources.record;
      btnRecord.BackgroundImageLayout = ImageLayout.Stretch;
      btnRecord.FlatStyle = FlatStyle.Popup;
      btnRecord.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnRecord.ForeColor = Color.Red;
      btnRecord.Location = new Point(136, 28);
      btnRecord.Margin = new Padding(9);
      btnRecord.Name = "btnRecord";
      btnRecord.Padding = new Padding(6);
      btnRecord.Size = new Size(96, 91);
      btnRecord.TabIndex = 4;
      btnRecord.UseVisualStyleBackColor = false;
      btnRecord.Click += btnRecord_Click;
      // 
      // btnCancel
      // 
      btnCancel.AutoSize = true;
      btnCancel.BackColor = Color.FromArgb(255, 128, 128);
      btnCancel.FlatStyle = FlatStyle.Flat;
      btnCancel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      btnCancel.ForeColor = Color.Black;
      btnCancel.Location = new Point(35, 151);
      btnCancel.Margin = new Padding(7);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(95, 42);
      btnCancel.TabIndex = 6;
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
      btnApply.Location = new Point(229, 151);
      btnApply.Margin = new Padding(7);
      btnApply.Name = "btnApply";
      btnApply.Size = new Size(95, 42);
      btnApply.TabIndex = 5;
      btnApply.Text = "Apply";
      btnApply.UseVisualStyleBackColor = false;
      btnApply.Click += btnApply_Click;
      // 
      // FrmGenRecord
      // 
      AutoScaleMode = AutoScaleMode.None;
      BackColor = Color.Black;
      ClientSize = new Size(360, 221);
      Controls.Add(btnCancel);
      Controls.Add(btnApply);
      Controls.Add(btnRecord);
      Font = new Font("Segoe UI", 12F);
      ForeColor = Color.FromArgb(255, 255, 192);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      Margin = new Padding(20, 21, 20, 21);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmGenRecord";
      Text = "Record Audio";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private Button btnRecord;
    private Button btnCancel;
    private Button btnApply;
  }
}
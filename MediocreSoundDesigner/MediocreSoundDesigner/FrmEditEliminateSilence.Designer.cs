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
            btnCancel = new Button();
            btnApply = new Button();
            nudMinDuration = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            nudThreshold = new NumericUpDown();
            label3 = new Label();
            cmbWhere = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)nudMinDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudThreshold).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.BackColor = SystemColors.Control;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(421, 231);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 25);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnApply
            // 
            btnApply.BackColor = SystemColors.Control;
            btnApply.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnApply.ForeColor = Color.Black;
            btnApply.Location = new Point(545, 231);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(103, 25);
            btnApply.TabIndex = 4;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // nudMinDuration
            // 
            nudMinDuration.DecimalPlaces = 2;
            nudMinDuration.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            nudMinDuration.Location = new Point(387, 70);
            nudMinDuration.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudMinDuration.Name = "nudMinDuration";
            nudMinDuration.Size = new Size(261, 43);
            nudMinDuration.TabIndex = 9;
            nudMinDuration.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(12, 70);
            label1.Name = "label1";
            label1.Size = new Size(262, 37);
            label1.TabIndex = 10;
            label1.Text = "Minimum Duration";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(12, 133);
            label2.Name = "label2";
            label2.Size = new Size(145, 37);
            label2.TabIndex = 12;
            label2.Text = "Threshold";
            // 
            // nudThreshold
            // 
            nudThreshold.DecimalPlaces = 4;
            nudThreshold.Increment = new decimal(new int[] { 25, 0, 0, 327680 });
            nudThreshold.Location = new Point(387, 133);
            nudThreshold.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudThreshold.Name = "nudThreshold";
            nudThreshold.Size = new Size(261, 43);
            nudThreshold.TabIndex = 11;
            nudThreshold.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 19);
            label3.Name = "label3";
            label3.Size = new Size(369, 37);
            label3.TabIndex = 13;
            label3.Text = "Find Silent Sections Where?";
            // 
            // cmbWhere
            // 
            cmbWhere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWhere.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbWhere.FormattingEnabled = true;
            cmbWhere.Items.AddRange(new object[] { "Beginning", "End", "Beginning and End", "Anywhere" });
            cmbWhere.Location = new Point(387, 19);
            cmbWhere.Name = "cmbWhere";
            cmbWhere.Size = new Size(261, 38);
            cmbWhere.TabIndex = 14;
            // 
            // FrmEditEliminateSilence
            // 
            AutoScaleDimensions = new SizeF(16F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(665, 277);
            Controls.Add(cmbWhere);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(nudThreshold);
            Controls.Add(label1);
            Controls.Add(nudMinDuration);
            Controls.Add(btnCancel);
            Controls.Add(btnApply);
            Name = "FrmEditEliminateSilence";
            Text = "FrmEditEliminateSilence";
            ((System.ComponentModel.ISupportInitialize)nudMinDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudThreshold).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
    private Button btnApply;
    private NumericUpDown nudMinDuration;
    private Label label1;
    private Label label2;
    private NumericUpDown nudThreshold;
    private Label label3;
    private ComboBox cmbWhere;
  }
}
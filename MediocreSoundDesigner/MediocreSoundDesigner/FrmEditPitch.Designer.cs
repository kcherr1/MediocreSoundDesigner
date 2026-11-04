namespace MediocreSoundDesigner
{
    /// <summary>
    /// Allows the pitch to be edited up or down
    /// </summary>
    partial class FrmEditPitch
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            nudSemitones = new NumericUpDown();
            label1 = new Label();
            btnApply = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)nudSemitones).BeginInit();
            SuspendLayout();
            // 
            // nudSemitones
            // 
            nudSemitones.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nudSemitones.Location = new Point(250, 54);
            nudSemitones.Margin = new Padding(7);
            nudSemitones.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            nudSemitones.Minimum = new decimal(new int[] { 24, 0, 0, int.MinValue });
            nudSemitones.Name = "nudSemitones";
            nudSemitones.Size = new Size(98, 39);
            nudSemitones.TabIndex = 0;
            nudSemitones.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(40, 56);
            label1.Margin = new Padding(7, 0, 7, 0);
            label1.Name = "label1";
            label1.Size = new Size(170, 37);
            label1.TabIndex = 1;
            label1.Text = "Pitch:";
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.Black;
            btnApply.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnApply.ForeColor = Color.Lime;
            btnApply.Location = new Point(234, 162);
            btnApply.Margin = new Padding(7);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(125, 47);
            btnApply.TabIndex = 2;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.BackColor = Color.Black;
            btnCancel.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.Red;
            btnCancel.Location = new Point(37, 162);
            btnCancel.Margin = new Padding(7);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(144, 47);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // FrmEditPitch
            // 
            AutoScaleDimensions = new SizeF(16F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(381, 231);
            Controls.Add(btnCancel);
            Controls.Add(btnApply);
            Controls.Add(label1);
            Controls.Add(nudSemitones);
            Margin = new Padding(16, 17, 16, 17);
            Name = "FrmEditPitch";
            Text = "Alter Pitch";
            ((System.ComponentModel.ISupportInitialize)nudSemitones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown nudSemitones;
        private Label label1;
        private Button btnApply;
        private Button btnCancel;
    }
}
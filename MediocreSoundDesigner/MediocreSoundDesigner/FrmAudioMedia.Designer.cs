namespace MediocreSoundDesigner
{
    partial class FrmAudioMedia
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
            doubleBufferedPanel1 = new DoubleBufferedPanel();
            lblTitle = new Label();
            lblMediaIn = new Label();
            lblMediaOut = new Label();
            comboMediaIn = new ComboBox();
            comboMediaOut = new ComboBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            doubleBufferedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // doubleBufferedPanel1
            // 
            doubleBufferedPanel1.BackColor = Color.DarkGray;
            doubleBufferedPanel1.Controls.Add(lblTitle);
            doubleBufferedPanel1.Controls.Add(lblMediaIn);
            doubleBufferedPanel1.Controls.Add(lblMediaOut);
            doubleBufferedPanel1.Controls.Add(comboMediaIn);
            doubleBufferedPanel1.Controls.Add(comboMediaOut);
            doubleBufferedPanel1.Controls.Add(btnConfirm);
            doubleBufferedPanel1.Controls.Add(btnCancel);
            doubleBufferedPanel1.Location = new Point(0, 0);
            doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            doubleBufferedPanel1.Size = new Size(583, 409);
            doubleBufferedPanel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(148, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(282, 50);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "Media Settings";
            lblTitle.Click += label1_Click;
            // 
            // lblMediaIn
            // 
            lblMediaIn.AutoSize = true;
            lblMediaIn.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMediaIn.Location = new Point(94, 212);
            lblMediaIn.Name = "lblMediaIn";
            lblMediaIn.Size = new Size(144, 32);
            lblMediaIn.TabIndex = 3;
            lblMediaIn.Text = "Media Input";
            lblMediaIn.Click += lblMediaIn_Click;
            // 
            // lblMediaOut
            // 
            lblMediaOut.AutoSize = true;
            lblMediaOut.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMediaOut.Location = new Point(94, 134);
            lblMediaOut.Name = "lblMediaOut";
            lblMediaOut.Size = new Size(164, 32);
            lblMediaOut.TabIndex = 2;
            lblMediaOut.Text = "Media Output";
            // 
            // comboMediaIn
            // 
            comboMediaIn.AllowDrop = true;
            comboMediaIn.FormattingEnabled = true;
            comboMediaIn.Location = new Point(302, 221);
            comboMediaIn.Name = "comboMediaIn";
            comboMediaIn.Size = new Size(196, 23);
            comboMediaIn.TabIndex = 1;
            comboMediaIn.SelectedIndexChanged += comboMediaIn_SelectedIndexChanged;
            // 
            // comboMediaOut
            // 
            comboMediaOut.AllowDrop = true;
            comboMediaOut.FormattingEnabled = true;
            comboMediaOut.Location = new Point(302, 143);
            comboMediaOut.Name = "comboMediaOut";
            comboMediaOut.Size = new Size(196, 23);
            comboMediaOut.TabIndex = 0;
            comboMediaOut.SelectedIndexChanged += comboMediaOut_SelectedIndexChanged;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(404, 290);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(94, 29);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += SubmitButton_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(302, 290);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += CancelButton_Click;
            // 
            // FrmAudioMedia
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(582, 408);
            Controls.Add(doubleBufferedPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAudioMedia";
            Text = "Media Settings";
            doubleBufferedPanel1.ResumeLayout(false);
            doubleBufferedPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DoubleBufferedPanel doubleBufferedPanel1;
        private Label lblMediaIn;
        private Label lblMediaOut;
        private ComboBox comboMediaIn;
        private ComboBox comboMediaOut;
        private Label lblTitle;
        private Button btnConfirm;
        private Button btnCancel;
    }
}
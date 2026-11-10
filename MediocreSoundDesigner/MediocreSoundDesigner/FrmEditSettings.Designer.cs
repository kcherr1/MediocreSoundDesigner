namespace MediocreSoundDesigner {
  partial class FrmEditSettings {
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
            btnApply = new Button();
            btnCancel = new Button();
            colorDialog = new ColorDialog();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btnColorWinBg = new Button();
            btnColorWinFg = new Button();
            btnColorTrackSamples = new Button();
            btnColorTrackSamplesDisabled = new Button();
            btnColorTrackBgSelected = new Button();
            btnColorTrackBgUnselected = new Button();
            btnPlayline = new Button();
            label7 = new Label();
            btnRevertToDefault = new Button();
            SuspendLayout();
            // 
            // btnApply
            // 
            btnApply.BackColor = SystemColors.Control;
            btnApply.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnApply.ForeColor = Color.Black;
            btnApply.Location = new Point(321, 491);
            btnApply.Margin = new Padding(7);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(130, 28);
            btnApply.TabIndex = 2;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = SystemColors.Control;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(175, 491);
            btnCancel.Margin = new Padding(7);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(132, 28);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 90);
            label1.Name = "label1";
            label1.Size = new Size(285, 37);
            label1.TabIndex = 4;
            label1.Text = "Window Background";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 141);
            label2.Name = "label2";
            label2.Size = new Size(263, 37);
            label2.TabIndex = 5;
            label2.Text = "Window Text Color";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 190);
            label3.Name = "label3";
            label3.Size = new Size(276, 37);
            label3.TabIndex = 6;
            label3.Text = "Track Samples Color";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 239);
            label4.Name = "label4";
            label4.Size = new Size(385, 37);
            label4.TabIndex = 7;
            label4.Text = "Track Samples Color Inactive";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 288);
            label5.Name = "label5";
            label5.Size = new Size(362, 37);
            label5.TabIndex = 8;
            label5.Text = "Track Selected Background";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 337);
            label6.Name = "label6";
            label6.Size = new Size(395, 37);
            label6.TabIndex = 9;
            label6.Text = "Track Unselected Background";
            // 
            // btnColorWinBg
            // 
            btnColorWinBg.BackColor = Color.Red;
            btnColorWinBg.Location = new Point(411, 92);
            btnColorWinBg.Name = "btnColorWinBg";
            btnColorWinBg.Size = new Size(40, 35);
            btnColorWinBg.TabIndex = 10;
            btnColorWinBg.UseVisualStyleBackColor = false;
            btnColorWinBg.Click += UpdateColorFromUserSelection;
            // 
            // btnColorWinFg
            // 
            btnColorWinFg.BackColor = Color.Red;
            btnColorWinFg.Location = new Point(411, 141);
            btnColorWinFg.Name = "btnColorWinFg";
            btnColorWinFg.Size = new Size(40, 35);
            btnColorWinFg.TabIndex = 11;
            btnColorWinFg.UseVisualStyleBackColor = false;
            btnColorWinFg.Click += UpdateColorFromUserSelection;
            // 
            // btnColorTrackSamples
            // 
            btnColorTrackSamples.BackColor = Color.Red;
            btnColorTrackSamples.Location = new Point(411, 190);
            btnColorTrackSamples.Name = "btnColorTrackSamples";
            btnColorTrackSamples.Size = new Size(40, 35);
            btnColorTrackSamples.TabIndex = 12;
            btnColorTrackSamples.UseVisualStyleBackColor = false;
            btnColorTrackSamples.Click += UpdateColorFromUserSelection;
            // 
            // btnColorTrackSamplesDisabled
            // 
            btnColorTrackSamplesDisabled.BackColor = Color.Red;
            btnColorTrackSamplesDisabled.Location = new Point(411, 239);
            btnColorTrackSamplesDisabled.Name = "btnColorTrackSamplesDisabled";
            btnColorTrackSamplesDisabled.Size = new Size(40, 35);
            btnColorTrackSamplesDisabled.TabIndex = 13;
            btnColorTrackSamplesDisabled.UseVisualStyleBackColor = false;
            btnColorTrackSamplesDisabled.Click += UpdateColorFromUserSelection;
            // 
            // btnColorTrackBgSelected
            // 
            btnColorTrackBgSelected.BackColor = Color.Red;
            btnColorTrackBgSelected.Location = new Point(411, 288);
            btnColorTrackBgSelected.Name = "btnColorTrackBgSelected";
            btnColorTrackBgSelected.Size = new Size(40, 35);
            btnColorTrackBgSelected.TabIndex = 14;
            btnColorTrackBgSelected.UseVisualStyleBackColor = false;
            btnColorTrackBgSelected.Click += UpdateColorFromUserSelection;
            // 
            // btnColorTrackBgUnselected
            // 
            btnColorTrackBgUnselected.BackColor = Color.Red;
            btnColorTrackBgUnselected.Location = new Point(411, 337);
            btnColorTrackBgUnselected.Name = "btnColorTrackBgUnselected";
            btnColorTrackBgUnselected.Size = new Size(40, 35);
            btnColorTrackBgUnselected.TabIndex = 15;
            btnColorTrackBgUnselected.UseVisualStyleBackColor = false;
            btnColorTrackBgUnselected.Click += UpdateColorFromUserSelection;
            // 
            // btnPlayline
            // 
            btnPlayline.BackColor = Color.Red;
            btnPlayline.Location = new Point(411, 388);
            btnPlayline.Name = "btnPlayline";
            btnPlayline.Size = new Size(40, 35);
            btnPlayline.TabIndex = 17;
            btnPlayline.UseVisualStyleBackColor = false;
            btnPlayline.Click += UpdateColorFromUserSelection;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 388);
            label7.Name = "label7";
            label7.Size = new Size(132, 37);
            label7.TabIndex = 16;
            label7.Text = "Play Line";
            // 
            // btnRevertToDefault
            // 
            btnRevertToDefault.AutoSize = true;
            btnRevertToDefault.BackColor = Color.White;
            btnRevertToDefault.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRevertToDefault.ForeColor = Color.Black;
            btnRevertToDefault.Location = new Point(119, 16);
            btnRevertToDefault.Margin = new Padding(7);
            btnRevertToDefault.Name = "btnRevertToDefault";
            btnRevertToDefault.Size = new Size(252, 47);
            btnRevertToDefault.TabIndex = 18;
            btnRevertToDefault.Text = "Revert To Default";
            btnRevertToDefault.UseVisualStyleBackColor = false;
            btnRevertToDefault.Click += btnRevertToDefault_Click;
            // 
            // FrmEditSettings
            // 
            AutoScaleDimensions = new SizeF(16F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(478, 547);
            Controls.Add(btnRevertToDefault);
            Controls.Add(btnPlayline);
            Controls.Add(label7);
            Controls.Add(btnColorTrackBgUnselected);
            Controls.Add(btnColorTrackBgSelected);
            Controls.Add(btnColorTrackSamplesDisabled);
            Controls.Add(btnColorTrackSamples);
            Controls.Add(btnColorWinFg);
            Controls.Add(btnColorWinBg);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnApply);
            ForeColor = Color.Black;
            Margin = new Padding(16, 17, 16, 17);
            Name = "FrmEditSettings";
            Text = "FrmEditVolume";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnApply;
    private Button btnCancel;
    private ColorDialog colorDialog;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Button btnColorWinBg;
    private Button btnColorWinFg;
    private Button btnColorTrackSamples;
    private Button btnColorTrackSamplesDisabled;
    private Button btnColorTrackBgSelected;
    private Button btnColorTrackBgUnselected;
    private Button btnPlayline;
    private Label label7;
    private Button btnRevertToDefault;
  }
}
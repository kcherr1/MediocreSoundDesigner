namespace MediocreSoundDesigner
{
    partial class FrmGenerate
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
            components = new System.ComponentModel.Container();
            txtBoxPrompt = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            lblTitle = new Label();
            lblPrompt = new Label();
            contextMenuStrip2 = new ContextMenuStrip(components);
            bttnSubmit = new Button();
            bttnCancel = new Button();
            checkBoxInstrumental = new CheckBox();
            lblUpdateText = new Label();
            lblPromptCharacterCount = new Label();
            SuspendLayout();
            // 
            // txtBoxPrompt
            // 
            txtBoxPrompt.Location = new Point(192, 103);
            txtBoxPrompt.MaxLength = 3000;
            txtBoxPrompt.Multiline = true;
            txtBoxPrompt.Name = "txtBoxPrompt";
            txtBoxPrompt.Size = new Size(487, 76);
            txtBoxPrompt.TabIndex = 0;
            txtBoxPrompt.TextChanged += txtBoxPrompt_TextChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(249, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(300, 47);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Generate Sounds";
            // 
            // lblPrompt
            // 
            lblPrompt.AutoSize = true;
            lblPrompt.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrompt.Location = new Point(52, 118);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(134, 45);
            lblPrompt.TabIndex = 3;
            lblPrompt.Text = "Prompt";
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(61, 4);
            // 
            // bttnSubmit
            // 
            bttnSubmit.Location = new Point(570, 394);
            bttnSubmit.Name = "bttnSubmit";
            bttnSubmit.Size = new Size(109, 29);
            bttnSubmit.TabIndex = 5;
            bttnSubmit.Text = "Submit";
            bttnSubmit.UseVisualStyleBackColor = true;
            bttnSubmit.Click += bttnSubmit_Click;
            // 
            // bttnCancel
            // 
            bttnCancel.DialogResult = DialogResult.Cancel;
            bttnCancel.Location = new Point(440, 394);
            bttnCancel.Name = "bttnCancel";
            bttnCancel.Size = new Size(109, 29);
            bttnCancel.TabIndex = 6;
            bttnCancel.Text = "Cancel";
            bttnCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxInstrumental
            // 
            checkBoxInstrumental.AutoSize = true;
            checkBoxInstrumental.CheckAlign = ContentAlignment.MiddleRight;
            checkBoxInstrumental.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxInstrumental.Location = new Point(192, 182);
            checkBoxInstrumental.Name = "checkBoxInstrumental";
            checkBoxInstrumental.Size = new Size(240, 36);
            checkBoxInstrumental.TabIndex = 7;
            checkBoxInstrumental.Text = "Instrumental Only";
            checkBoxInstrumental.UseVisualStyleBackColor = true;
            // 
            // lblUpdateText
            // 
            lblUpdateText.AutoSize = true;
            lblUpdateText.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUpdateText.Location = new Point(333, 303);
            lblUpdateText.Name = "lblUpdateText";
            lblUpdateText.Size = new Size(0, 32);
            lblUpdateText.TabIndex = 8;
            // 
            // lblPromptCharacterCount
            // 
            lblPromptCharacterCount.BackColor = SystemColors.ActiveBorder;
            lblPromptCharacterCount.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPromptCharacterCount.Location = new Point(579, 182);
            lblPromptCharacterCount.Name = "lblPromptCharacterCount";
            lblPromptCharacterCount.Size = new Size(100, 23);
            lblPromptCharacterCount.TabIndex = 9;
            // 
            // FrmGenerate
            // 
            AcceptButton = bttnSubmit;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            CancelButton = bttnCancel;
            ClientSize = new Size(800, 450);
            Controls.Add(lblUpdateText);
            Controls.Add(lblPromptCharacterCount);
            Controls.Add(checkBoxInstrumental);
            Controls.Add(bttnCancel);
            Controls.Add(bttnSubmit);
            Controls.Add(lblPrompt);
            Controls.Add(lblTitle);
            Controls.Add(txtBoxPrompt);
            Name = "FrmGenerate";
            Text = "Generate Sounds";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBoxPrompt;
        private ContextMenuStrip contextMenuStrip1;
        private Label lblTitle;
        private Label lblPrompt;
        private ContextMenuStrip contextMenuStrip2;
        private Button bttnSubmit;
        private Button bttnCancel;
        private CheckBox checkBoxInstrumental;
        private Label lblUpdateText;
        private Label lblPromptCharacterCount;
    }
}
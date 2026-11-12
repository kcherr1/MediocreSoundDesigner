namespace MediocreSoundDesigner
{
    public partial class FrmGenerate : Form
    {
        public FrmGenerate()
        {
            InitializeComponent();
            ApplySettings();
            UpdatePromptCharacterCount();
        }

        private void txtBoxPrompt_TextChanged(object sender, EventArgs e)
        {
            UpdatePromptCharacterCount();
        }

        private void UpdatePromptCharacterCount()
        {
            lblPromptCharacterCount.Text = $"{txtBoxPrompt.TextLength}/80";
        }

        private void ApplySettings()
        {
            BackColor = Settings.Color.WinBg;
            ForeColor = Settings.Color.WinFg;

            foreach (Control control in Controls)
            {
                ApplySettings(control);
            }
        }

        private static void ApplySettings(Control control)
        {
            control.ForeColor = Settings.Color.WinFg;

            foreach (Control child in control.Controls)
            {
                ApplySettings(child);
            }
        }


        private async void bttnSubmit_Click(object sender, EventArgs e)
        {
            string prompt = txtBoxPrompt.Text.Trim();
            if (prompt.Length == 0)
            {
                lblUpdateText.Text = "Enter a prompt.";
                txtBoxPrompt.Focus();
                return;
            }

            if (Owner is not FrmMain mainForm)
            {
                lblUpdateText.Text = "Unable to access the main form.";
                return;
            }

            SetSubmittingState(true);
            lblUpdateText.Text = "Generating...";

            try
            {
                await mainForm.GenerateAndImportUdioTrackAsync(
                    prompt,
                    makeInstrumental: checkBoxInstrumental.Checked);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or TimeoutException)
            {
                lblUpdateText.Text = ex.Message;
            }
            catch (TaskCanceledException)
            {
                lblUpdateText.Text = "The request timed out.";
            }
            finally
            {
                SetSubmittingState(false);
            }
        }

        private void SetSubmittingState(bool submitting)
        {
            txtBoxPrompt.Enabled = !submitting;
            checkBoxInstrumental.Enabled = !submitting;
            bttnSubmit.Enabled = !submitting;
            bttnCancel.Enabled = !submitting;
        }

    }
}

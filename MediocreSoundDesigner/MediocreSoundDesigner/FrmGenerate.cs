using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MediocreSoundDesigner
{
    public partial class FrmGenerate : Form
    {
        private static readonly HttpClient httpClient = new();
        private const string endpoint = "https://udioapi.pro/api/v2/generate";

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

            string apiKey = Environment.GetEnvironmentVariable("UDIOAPI_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                lblUpdateText.Text = "API_KEY is not configured.";
                return;
            }

            SetSubmittingState(true);
            lblUpdateText.Text = "Generating...";

            try
            {
                using HttpRequestMessage request = new(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(new
                    {
                        model = "chirp-v6",
                        gpt_description_prompt = prompt,
                        make_instrumental = checkBoxInstrumental.Checked
                    })
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                using HttpResponseMessage response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    lblUpdateText.Text = FormatError(response.StatusCode, message);
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException ex)
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

        private static string FormatError(HttpStatusCode statusCode, string responseBody)
        {
            string message = responseBody.Trim();
            return string.IsNullOrWhiteSpace(message)
                ? $"Request failed ({(int)statusCode} {statusCode})."
                : $"{(int)statusCode}: {message}";
        }
    }
}

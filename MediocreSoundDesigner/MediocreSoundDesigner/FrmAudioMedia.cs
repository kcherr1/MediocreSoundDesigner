using NAudio.Wave;

namespace MediocreSoundDesigner
{
    public partial class FrmAudioMedia : Form
    {
        public FrmAudioMedia()
        {
            InitializeComponent();
            LoadAudioDevices();
        }

        private void LoadAudioDevices()
        {
            try
            {
                AddCaptureDevices();
                AddOutputDevices();
            } catch (Exception ex)
            {
                MessageBox.Show("Error loading audio devices: " + ex.Message);
            }
        }

        private void AddCaptureDevices()
        {
            for (int deviceNumber = 0; deviceNumber < WaveIn.DeviceCount; deviceNumber++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(deviceNumber);
                comboMediaIn.Items.Add(new AudioDevice(deviceNumber, capabilities.ProductName));
            }

            SelectDevice(comboMediaIn, Settings.AudioInputDeviceNumber);
        }

        private void AddOutputDevices()
        {
            for (int deviceNumber = 0; deviceNumber < WaveOut.DeviceCount; deviceNumber++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(deviceNumber);
                comboMediaOut.Items.Add(new AudioDevice(deviceNumber, capabilities.ProductName));
            }

            SelectDevice(comboMediaOut, Settings.AudioOutputDeviceNumber);
        }

        private static void SelectDevice(ComboBox comboBox, int deviceNumber)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = Math.Clamp(deviceNumber, 0, comboBox.Items.Count - 1);
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (comboMediaIn.SelectedItem is not AudioDevice inputDevice ||
                comboMediaOut.SelectedItem is not AudioDevice outputDevice)
            {
                MessageBox.Show("Please select both input and output devices.");
                return;
            }

            Settings.AudioInputDeviceNumber = inputDevice.Number;
            Settings.AudioOutputDeviceNumber = outputDevice.Number;
            Settings.SaveToFile();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void comboMediaOut_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblMediaIn_Click(object sender, EventArgs e)
        {

        }

        private void comboMediaIn_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private sealed record AudioDevice(int Number, string Name)
        {
            public override string ToString() => Name;
        }
    }
}

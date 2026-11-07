using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

                MMDeviceCollection inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                AddAudioDevicesToComboBox(comboMediaIn, inputDevices);
                MMDeviceCollection outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                AddAudioDevicesToComboBox(comboMediaOut, outputDevices);
            } catch (Exception ex)
            {
                MessageBox.Show("Error loading audio devices: " + ex.Message);
            }
        }

        private void AddAudioDevicesToComboBox(ComboBox comboBox, MMDeviceCollection devices)
        {
            foreach (var device in devices)
            {
                comboBox.Items.Add(device.FriendlyName);
            }
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
    }
}

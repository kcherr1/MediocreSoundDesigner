using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace MediocreSoundDesigner
{
  public partial class GenerateTone : Form
  {
    private const int SAMPLE_RATE = 44100;

    public float[] GeneratedSamples { get; private set; } = Array.Empty<float>();
    public int FrequencyHz => (int)numericUpDown1.Value;
    
    public double StartTimeSeconds => (double)nudStartTime.Value;
    public WaveFormat GeneratedFormat => WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, 1);

    public string WaveformName => cmbWaveform.SelectedItem?.ToString() ?? "Sine";

    public GenerateTone()
    {
      InitializeComponent();
      cmbWaveform.SelectedIndex = 0;
    }

    private void btnCreate_Click(object sender, EventArgs e)
    {
      int frequency = FrequencyHz;
      int sampleCount = (int)(SAMPLE_RATE * (double)nudDuration.Value);
      string waveform = cmbWaveform.SelectedItem?.ToString() ?? "Sine";

      GeneratedSamples = new float[sampleCount];

      for (int i = 0; i < sampleCount; i++)
      {
        double cycles = (double)frequency * i / SAMPLE_RATE;
        double phase = 2 * Math.PI * cycles;

        double value = waveform switch
        {
          "Square" => Math.Sin(phase) >= 0 ? 1.0 : -1.0,
          "Triangle" => (2.0 / Math.PI) * Math.Asin(Math.Sin(phase)),
          "Sawtooth" => 2.0 * (cycles - Math.Floor(cycles + 0.5)),
          _ => Math.Sin(phase)
        };

        GeneratedSamples[i] = (float)(value * 0.5);
      }

      this.DialogResult = DialogResult.OK;
    }
    private void numericUpDown2_ValueChanged(object sender, EventArgs e)
    {
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void GenerateTone_Load(object sender, EventArgs e) { }
    private void lblWaveform_Click(object sender, EventArgs e) { }
    private void lblFrequency_Click(object sender, EventArgs e) { }
    private void lblDuration_Click(object sender, EventArgs e) { }
    private void nudDuration_ValueChanged(object sender, EventArgs e) { }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

    private void cmbWaveform_SelectedIndexChanged(object sender, EventArgs e) { }
  }
}
using System;
using System.Windows.Forms;

namespace MediocreSoundDesigner
{
  public partial class FrmGenerateSynth : Form
  {
    public FrmGenerateSynth()
    {
      InitializeComponent();

      if (cmbInstrument.Items.Count > 0)
        cmbInstrument.SelectedIndex = 0;

      if (cmbNote.Items.Count > 0)
        cmbNote.SelectedIndex = 0;
    }

    public string Instrument =>
      cmbInstrument.SelectedItem?.ToString() ?? "Synth Lead 1";

    public string Note =>
      cmbNote.SelectedItem?.ToString() ?? "C";

    public int Octave => (int)nudOctave.Value;

    public double StartTimeSeconds => (double)nudStartTime.Value;

    public double DurationSeconds => (double)nudDuration.Value;

    private void btnCreate_Click(object sender, EventArgs e)
    {
      // The Create button's DialogResult is already set to OK in the Designer.
    }

    private void lblRootnote_Click(object sender, EventArgs e)
    {
      // Empty handler retained because the Designer currently wires this event.
    }
  }
}
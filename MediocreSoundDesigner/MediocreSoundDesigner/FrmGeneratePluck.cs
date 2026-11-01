namespace MediocreSoundDesigner
{
  public partial class FrmGeneratePluck : Form
  {
    public string Instrument => cmbInstrument.SelectedItem?.ToString() ?? "Acoustic Guitar";
    public string Note => cmbNote.SelectedItem?.ToString().Trim() ?? "C";
    public int Octave => (int)nudOctave.Value;
    public double StartTimeSeconds => (double)nudStartTime.Value;
    public double DurationSeconds => (double)nudDuration.Value;

    public FrmGeneratePluck()
    {
      InitializeComponent();

      if (cmbInstrument.Items.Count > 0)
      {
        cmbInstrument.SelectedIndex = 0;
      }

      if (cmbNote.Items.Count > 0)
      {
        cmbNote.SelectedIndex = 0;
      }
    }

    private void nudStartTime_ValueChanged(object sender, EventArgs e)
    {
    }
  }
}
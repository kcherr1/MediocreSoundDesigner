namespace MediocreSoundDesigner
{
  public partial class FrmGenerateBeat : Form
  {
    public string DrumKit => cmbDrumKit.SelectedItem?.ToString() ?? "Standard";
    public string Rhythm => cmbRhythm.SelectedItem?.ToString() ?? "Rock";
    public int Tempo => (int)nudTempo.Value;
    public int Bars => (int)nudBars.Value;
    public double StartTimeSeconds => (double)nudStartTime.Value;

    public FrmGenerateBeat()
    {
      InitializeComponent();

      if (cmbDrumKit.Items.Count > 0)
      {
        cmbDrumKit.SelectedIndex = 0;
      }

      if (cmbRhythm.Items.Count > 0)
      {
        cmbRhythm.SelectedIndex = 0;
      }
    }

    private void cmbDrumKit_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
  }
}
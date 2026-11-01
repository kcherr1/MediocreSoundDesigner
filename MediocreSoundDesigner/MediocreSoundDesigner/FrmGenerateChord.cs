namespace MediocreSoundDesigner
{
  public partial class FrmGenerateChord : Form
  {
    public string RootNote => cmbRootnote.SelectedItem?.ToString() ?? "C";
    public string ChordType => cmbChordType.SelectedItem?.ToString() ?? "Major";
    public int Octave => (int)nudOctave.Value;
    public double StartTimeSeconds => (double)nudStartTime.Value;
    public double DurationSeconds => (double)nudDuration.Value;

    public FrmGenerateChord()
    {
      InitializeComponent();

      if (cmbRootnote.Items.Count > 0)
      {
        cmbRootnote.SelectedIndex = 0;
      }

      if (cmbChordType.Items.Count > 0)
      {
        cmbChordType.SelectedIndex = 0;
      }
    }

    private void FrmGenerateChord_Load(object sender, EventArgs e)
    {
    }

    private void cmbRootnote_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void lblChordType_Click(object sender, EventArgs e)
    {
    }
  }
}
namespace MediocreSoundDesigner;

public partial class FrmEditEliminateSilence : FrmEditMaster
{
  public FrmEditEliminateSilence()
  {
    InitializeComponent();
  }

  protected override void Init()
  {
    base.Init();
    cmbWhere.SelectedIndex = 0;
  }

  protected override void Apply()
  {
    SampleRegion region = cmbWhere.SelectedIndex switch
    {
      0 => SampleRegion.BEGINNING_ONLY,
      1 => SampleRegion.END_ONLY,
      2 => SampleRegion.BEGINNING_AND_END,
      3 => SampleRegion.ANYWHERE,
       _ => SampleRegion.BEGINNING_ONLY
    };
    track.EliminateSilence(region, (float)nudMinDuration.Value, (float)nudThreshold.Value);
  }

  private void label1_Click(object sender, EventArgs e)
  {

  }

  private void nudMinDuration_ValueChanged(object sender, EventArgs e)
  {

  }

  private void nudThreshold_ValueChanged(object sender, EventArgs e)
  {

  }

  private void btnApply_Click_1(object sender, EventArgs e)
  {

  }

  private void label2_Click(object sender, EventArgs e)
  {

  }

  private void cmbWhere_SelectedIndexChanged(object sender, EventArgs e)
  {

  }
}

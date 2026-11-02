namespace MediocreSoundDesigner;
/// <summary>
/// Manages eliminating silence from the audio track.
/// </summary>
public partial class FrmEditEliminateSilence : FrmEditMaster {
  /// <summary>
  /// Creates the dialog and builds the controls
  /// </summary>
  public FrmEditEliminateSilence() {
    InitializeComponent();
  }
  /// <inheritdoc/>
  protected override void Init() {
    base.Init();
    cmbWhere.SelectedIndex = 0;
  }

  /// <summary>
  /// Applies the silence removal
  /// </summary>
  protected override void Apply() {
    SampleRegion region = cmbWhere.SelectedIndex switch {
      0 => SampleRegion.BEGINNING_ONLY,
      1 => SampleRegion.END_ONLY,
      2 => SampleRegion.BEGINNING_AND_END,
      3 => SampleRegion.ANYWHERE,
      _ => throw new InvalidOperationException("Unexpected option selected in the 'where' list."),
    };
    track.EliminateSilence(region, (float)nudMinDuration.Value, (float)nudThreshold.Value);
  }
}

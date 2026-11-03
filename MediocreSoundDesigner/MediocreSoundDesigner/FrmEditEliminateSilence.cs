using System.ComponentModel;

namespace MediocreSoundDesigner;

public partial class FrmEditEliminateSilence : FrmEditMaster
{
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // added to avoid runtime variable serialization error in designer
    public List<Track> SelectedTracks { get; set; } = new();

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
            3 => SampleRegion.ANYWHERE
        };

        float minDuration = (float)nudMinDuration.Value;
        float threshold = (float)nudThreshold.Value;

        if (SelectedTracks.Count > 0)
        {
            foreach (Track t in SelectedTracks)
            {
                t.EliminateSilence(region, minDuration, threshold);
                t.PanDrawSamples?.Invalidate();
            }
        }
        else
        {
            track?.EliminateSilence(region, minDuration, threshold);
            track?.PanDrawSamples?.Invalidate();
        }
    }
}

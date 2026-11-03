using System.ComponentModel;

namespace MediocreSoundDesigner;

public partial class FrmEditVolume : FrmEditMaster
{
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<Track> SelectedTracks { get; set; } = new();

    public FrmEditVolume()
    {
        InitializeComponent();
    }

    protected override void Init()
    {
        base.Init();
        nudVolume.Value = 100;
    }

    protected override void Apply()
    {
        float factor = (float)nudVolume.Value / 100.0f;
        if (SelectedTracks.Count > 0)
        {
            foreach (Track t in SelectedTracks)
            {
                t.ApplyVolume(factor);
                t.PanDrawSamples?.Invalidate();
            }
        }
        else
        {
            track?.ApplyVolume(factor);
            track?.PanDrawSamples?.Invalidate();
        }
    }
}

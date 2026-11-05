namespace MediocreSoundDesigner;

// name column for tracks, drawn to the left of its audio
// a mute dot bright being active and dim being disabled
public class TrackHeader : Panel {
  private readonly Track track;
  private readonly Func<bool> isSelected;

  // clicking on the track selects it
  public event EventHandler HeaderClicked;
  // clicking the dot mutes or unmutes the track
  public event EventHandler MuteToggled;

  public TrackHeader(Track track, Func<bool> isSelected) {
    this.track = track;
    this.isSelected = isSelected;
    DoubleBuffered = true;
    ResizeRedraw = true;
  }

  private Rectangle GetDotBounds() {
    int size = LogicalToDeviceUnits(14);
    int pad = LogicalToDeviceUnits(10);
    return new Rectangle(Width - size - pad, (Height - size) / 2, size, size);
  }

  protected override void OnPaint(PaintEventArgs e) {
    base.OnPaint(e);
    Graphics g = e.Graphics;
    bool selected = isSelected?.Invoke() ?? false;
    g.Clear(selected ? Settings.Color.TrackHeader_Selected : Settings.Color.TrackHeader_Bg);

    //  separator between the name column and the audio
    using (Pen separator = new(Settings.Color.TopBar_Border)) {
      g.DrawLine(separator, Width - 1, 0, Width - 1, Height);
    }

    // mute dot
    Rectangle dot = GetDotBounds();
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    using (SolidBrush fill = new(track.IsDisabled ? Settings.Color.MuteDot_Off : Settings.Color.MuteDot_On)) {
      g.FillEllipse(fill, dot);
    }

    // track name
    int pad = LogicalToDeviceUnits(10);
    Rectangle textBounds = new(pad, 0, Math.Max(1, dot.Left - pad * 2), Height);
    TextRenderer.DrawText(g, track.Name ?? "", Font, textBounds,
      track.IsDisabled ? Color.Gray : Settings.Color.TrackHeader_Text,
      TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
  }

  protected override void OnMouseDown(MouseEventArgs e) {
    base.OnMouseDown(e);
    if (e.Button != MouseButtons.Left) {
      return;
    }
    Rectangle hit = GetDotBounds();
    hit.Inflate(LogicalToDeviceUnits(6), LogicalToDeviceUnits(6));
    if (hit.Contains(e.Location)) {
      MuteToggled?.Invoke(this, EventArgs.Empty);
    }
    else {
      HeaderClicked?.Invoke(this, EventArgs.Empty);
    }
  }
}

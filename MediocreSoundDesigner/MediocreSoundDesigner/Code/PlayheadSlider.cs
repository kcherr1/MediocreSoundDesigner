using System.ComponentModel;
using System.Windows.Forms.VisualStyles;

namespace MediocreSoundDesigner;

/// <summary>
/// Strip that sits above the tracks and shows a trackbar-style thumb (same look as the
/// volume slider) on top of the playline. Click or drag anywhere on it to move the playhead.
/// Positions are in "content" pixels (same units as the playline / waveform); OriginX keeps
/// it lined up when the track panel is scrolled horizontally (see OriginX).
/// </summary>
public class PlayheadSlider : Control
{
    private bool dragging;
    private int contentX;

    public event EventHandler<int> Scrubbing;      // fires continuously while dragging
    public event EventHandler<int> ScrubCompleted; // fires once on mouse release

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int ContentX
    {
        get => contentX;
        set { if (contentX != value) { contentX = value; Invalidate(); } }
    }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int MaxContentX { get; set; }

    // x (in this control's coordinates) where content pixel 0 currently is on screen;
    // takes scrolling and panel borders into account, so the thumb lines up with the playline
    private int originX;
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int OriginX
    {
        get => originX;
        set { if (originX != value) { originX = value; Invalidate(); } }
    }

    public PlayheadSlider()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Height = 26;
        TabStop = false;
    }

    public void ResetToStart()
    {
        dragging = false;
        contentX = 0;
        originX = 0;
        MaxContentX = 0;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.Clear(BackColor);

        int centerX = OriginX + ContentX;
        TrackBarThumbState state = dragging ? TrackBarThumbState.Pressed : TrackBarThumbState.Normal;

        if (TrackBarRenderer.IsSupported)
        {
            Size ts = TrackBarRenderer.GetBottomPointingThumbSize(g, state);
            Rectangle thumb = new(centerX - ts.Width / 2, Height - ts.Height, ts.Width, ts.Height);
            Rectangle rail = new(0, thumb.Y + (ts.Height - 4) / 2, Width, 4);
            TrackBarRenderer.DrawHorizontalTrack(g, rail);
            TrackBarRenderer.DrawBottomPointingThumb(g, thumb, state);
        }
        else
        {
            // fallback if visual styles are off
            using Pen rail = new(ForeColor);
            g.DrawLine(rail, 0, Height / 2, Width, Height / 2);
            Point[] shield = { new(centerX - 5, 3), new(centerX + 5, 3), new(centerX + 5, Height - 8),
                               new(centerX, Height - 1), new(centerX - 5, Height - 8) };
            g.FillPolygon(Brushes.RoyalBlue, shield);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left) return;
        dragging = true;
        MoveTo(e.X);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (dragging) MoveTo(e.X);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (!dragging || e.Button != MouseButtons.Left) return;
        dragging = false;
        Invalidate();
        ScrubCompleted?.Invoke(this, ContentX);
    }

    private void MoveTo(int mouseX)
    {
        ContentX = Math.Clamp(mouseX - OriginX, 0, Math.Max(0, MaxContentX));
        Scrubbing?.Invoke(this, ContentX);
    }
}
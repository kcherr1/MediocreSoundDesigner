namespace MediocreSoundDesigner;

// Layout for the top bar and the track name column.
public partial class FrmMain {
  private Panel panTopBar;
  private int nextTrackNumber = 1;

  private const int HEADER_WIDTH = 150;   // track name column
  private const int TIMESCALE_WIDTH = 320; // zoom slider

  private int HeaderWidth => LogicalToDeviceUnits(HEADER_WIDTH);

  
  private class BufferedPanel : Panel {
    public BufferedPanel() {
      DoubleBuffered = true;
    }
  }

  // A thick bar strip under the menu. For now it only holds the play button and the slider 
  private void BuildTopBar() {
    panTopBar = new BufferedPanel { Name = "panTopBar" };
    panTopBar.Paint += panTopBar_Paint;
    Controls.Add(panTopBar);
    panTopBar.Controls.Add(btnPlay);
    panTopBar.Controls.Add(trkDrawTimescale);
  }

  private void ApplyTopBarSettings() {
    if (panTopBar is null) {
      return;
    }
    panTopBar.BackColor = Settings.Color.TopBar_Bg;
    trkDrawTimescale.BackColor = Settings.Color.TopBar_Bg;
    panTopBar.Invalidate();
  }

  private void panTopBar_Paint(object sender, PaintEventArgs e) {
    using Pen border = new(Settings.Color.TopBar_Border);
    e.Graphics.DrawRectangle(border, 0, 0, panTopBar.Width - 1, panTopBar.Height - 1);
  }

  private void LayoutTopBar() {
    if (panTopBar is null) {
      return;
    }
    int gap = LogicalToDeviceUnits(6);
    int pad = LogicalToDeviceUnits(14);

    // line the bar up with the track area underneath it
    panTopBar.Left = panTracks.Left;
    panTopBar.Width = panTracks.Width;
    panTopBar.Top = menu.Bottom;
    panTopBar.Height = Math.Max(1, panTracks.Top - menu.Bottom - gap);

    btnPlay.Left = pad;
    btnPlay.Top = Math.Max(0, (panTopBar.Height - btnPlay.Height) / 2);

    trkDrawTimescale.Left = btnPlay.Right + pad;
    trkDrawTimescale.Width = Math.Min(LogicalToDeviceUnits(TIMESCALE_WIDTH), Math.Max(1, panTopBar.Width - trkDrawTimescale.Left - pad));
    trkDrawTimescale.Top = Math.Max(0, (panTopBar.Height - trkDrawTimescale.Height) / 2);
  }

  // ---- track name column -----------------------------------------------------
  private void CreateTrackHeader(Track track) {
    if (string.IsNullOrWhiteSpace(track.Name)) {
      track.Name = $"Track {nextTrackNumber}";
      nextTrackNumber++;
    }

    TrackHeader header = new(track, () => trackManager.SelectedTrack == track);
    header.HeaderClicked += (sender, e) => {
      trackManager.SelectTrack(track);
    };
    header.MuteToggled += (sender, e) => {
      track.ToggleDisabled();
      track.PanDrawSamples.Invalidate();
      header.Invalidate();
    };
    panTracks.Controls.Add(header);
    track.LinkHeader(header);
  }

  private void RemoveHeader(Track track) {
    if (track.PanHeader is null) {
      return;
    }
    panTracks.Controls.Remove(track.PanHeader);
    track.PanHeader.Dispose();
  }
}

namespace MediocreSoundDesigner;

public partial class FrmEditMaster : Form
{
  protected Track track;

  public FrmEditMaster()
  {
    InitializeComponent();
  }

  public virtual void SetSound(Track sound)
  {
    track = sound;
    Init();
  }

  protected void ApplySettings()
  {
    BackColor = Settings.Color.WinBg;
    ForeColor = Settings.Color.WinFg;
  }

  public void btnCancel_Click(object sender, EventArgs e)
  {
    Cancel();
    DialogResult = DialogResult.Cancel;
    Close();
  }

  public void btnApply_Click(object sender, EventArgs e)
  {
    Apply();
    track?.PanDrawSamples?.Invalidate();
    DialogResult = DialogResult.OK;
    Close();
  }

  protected virtual void Init()
  {
  }

  protected virtual void Apply() { }

  protected virtual void Cancel() { }

  public static void StyleDialog(Form form, int minHeight = 400)
  {
    // Kept for compatibility. Form size and control layout belong to the Designer.
    _ = minHeight;

    form.FormBorderStyle = FormBorderStyle.FixedDialog;
    form.MaximizeBox = false;
    form.MinimizeBox = false;
    form.BackColor = Color.Black;
    form.ForeColor = Color.FromArgb(255, 255, 192);

    foreach (Control control in form.Controls)
    {
      if (control is Label)
      {
        control.ForeColor = form.ForeColor;
      }
      else if (control is TextBox || control is NumericUpDown || control is ComboBox)
      {
        control.BackColor = Color.DimGray;
        control.ForeColor = Color.White;
      }
      else if (control is Button button && button.Text != "")
      {
        button.FlatStyle = FlatStyle.Flat;
        button.ForeColor = Color.White;
        button.BackColor = button.Text switch
        {
          "Cancel" => Color.Firebrick,
          "Apply" or "OK" or "Create" => Color.SeaGreen,
          _ => Color.DimGray
        };
      }
    }
  }

  public static class Popup
  {
    public static DialogResult Show(
        string text,
        string title = "Medicore Sound Designer",
        MessageBoxButtons buttons = MessageBoxButtons.OK,
        MessageBoxIcon icon = MessageBoxIcon.None)
    {
      Form form = new();
      form.Text = title;
      form.ClientSize = new Size(520, 250);
      form.StartPosition = FormStartPosition.CenterScreen;
      form.Font = new Font("Segoe UI", 12);

      Label label = new();
      label.Text = text;
      label.Size = new Size(440, 130);
      label.Location = new Point(40, 30);
      form.Controls.Add(label);

      string[] names = buttons == MessageBoxButtons.OK
          ? new[] { "OK" }
          : new[] { "Yes", "No", "Cancel" };

      for (int i = 0; i < names.Length; i++)
      {
        Button button = new();
        button.Text = names[i];
        button.DialogResult = Enum.Parse<DialogResult>(names[i]);
        button.Size = new Size(110, 42);
        button.Location = new Point(270 - names.Length * 75 + i * 150, 180);
        form.Controls.Add(button);
      }

      StyleDialog(form, 250);
      DialogResult result = form.ShowDialog();
      form.Dispose();
      return result;
    }

    public static DialogResult Show(
        IWin32Window owner,
        string text,
        string title = "Medicore Sound Designner",
        MessageBoxButtons buttons = MessageBoxButtons.OK,
        MessageBoxIcon icon = MessageBoxIcon.None)
    {
      return Show(text, title, buttons, icon);
    }
  }
}
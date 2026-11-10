namespace MediocreSoundDesigner;

public partial class FrmEditMaster : Form {
  // fields
  protected Track track;

  // ctor
  public FrmEditMaster() {
    InitializeComponent();
  }

  // sound
  public virtual void SetSound(Track sound) {
    this.track = sound;
    Init();
  }

  // gui
  protected void ApplySettings() {
    BackColor = Settings.Color.WinBg;
    ForeColor = Settings.Color.WinFg;
  }

  public void btnCancel_Click(object sender, EventArgs e) {
    Cancel();
    DialogResult = DialogResult.Cancel;
    Close();
  }

  public void btnApply_Click(object sender, EventArgs e) {
    Apply();
    track?.PanDrawSamples?.Invalidate();
    DialogResult = DialogResult.OK;
    Close();
  }

  // override in subclass if needed
  protected virtual void Init() { ApplySettings(); }
  protected virtual void Apply() { }
  protected virtual void Cancel() { }


  protected override void OnLoad(EventArgs e)
  {
        StyleDialog(this);
		base.OnLoad(e);
  }

  public static void StyleDialog(Form form, int minHeight = 400)
  {
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.MaximizeBox = false;
        form.MinimizeBox = false;
        form.BackColor = Color.Black;
        form.ForeColor = Color.FromArgb(255, 255, 192);
        form.Font = new Font("Segoe UI", 12);

        int width = Math.Max(form.ClientSize.Width, 520);
        int height = Math.Max(form.ClientSize.Height, minHeight);
        int moveX = (width - form.ClientSize.Width) / 2;
        int moveY = (height - form.ClientSize.Height) / 2;
        form.ClientSize = new Size(width, height);

        foreach(Control c in form.Controls)
        {
            c.Left += moveX;
            c.Top += moveY;
            c.Font = form.Font;

            if (c is TextBox || c is NumericUpDown || c is ComboBox || (c is Button && c.Text !=""))
            {
                c.BackColor = Color.DimGray;
                c.ForeColor = Color.White;
            }
        }

  }

  public static class Popup
  {
    public static DialogResult Show(string text, string title = "Medicore Sound Designer", 
        MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            Form form = new Form();
            form.Text = title;
            form.ClientSize = new Size(520, 250);
            form.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = text;
            label.Size = new Size(440, 130);
            label.Location = new Point(40, 30);
            form.Controls.Add(label);

            string[] names = buttons == MessageBoxButtons.OK ? new[] { "OK" } : new[] { "Yes", "No", "Cancel" };

            for (int i = 0; i < names.Length; i++)
            {
                Button button = new Button();
                button.Text = names[i];

            }

        }
  }

}

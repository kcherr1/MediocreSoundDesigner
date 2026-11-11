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
        //form.ClientSize = new Size(Math.Max(form.ClientSize.Width, 520), Math.Max(form.ClientSize.Height, minHeight));

        int width = Math.Max(form.ClientSize.Width, 520);
        int height = Math.Max(form.ClientSize.Height, minHeight);
        int moveX = (width - form.ClientSize.Width) / 2;
        int moveY = (height - form.ClientSize.Height) / 2;
        form.ClientSize = new Size(width, height);

        Button apply = null;
        Button cancel = null;
        int buttonCount = 0;

        foreach(Control c in form.Controls)
        {
            c.Left += moveX;
            c.Top += moveY;
            c.Font = form.Font;

            if (c is Label)
            {
                c.ForeColor = form.ForeColor;
            }

            else if (c is TextBox || c is NumericUpDown || c is ComboBox )
            {
                c.BackColor = Color.DimGray;
                c.ForeColor = Color.White;
            }
            else if (c is Button button && button.Text != "")
            {
                buttonCount++;
                button.FlatStyle = FlatStyle.Flat;
                button.AutoSize = false;
                button.Size = new Size(130, 44);
                button.ForeColor = Color.White;
                button.BackColor = Color.DimGray;


                if (button.Text == "Cancel")
                {
                    button.BackColor = Color.SeaGreen;
                    apply = button;
                }
            }
        }

        foreach (Control label in form.Controls)
        {
            if (label is not Label) continue;

            Control nearest = null;
            foreach(Control input in form.Controls)
            {
                if (input is TextBox || input is NumericUpDown || input is ComboBox)
                {
                    if (nearest == null || Math.Abs(input.Top - label.Top) < Math.Abs(nearest.Top - label.Top))
                        nearest = input;
                }
            }

            if (nearest != null)
            {
                label.Top = nearest.Top + (nearest.Height - label.Height) / 2;
                if (label.Right + 10 > nearest.Left)
                    label.Left = nearest.Left - label.Width - 15;
            }

        }

        if (buttonCount == 2 && apply != null && cancel != null)
        {
            int left = (form.ClientSize.Width - 130 * 2 - 80) / 2;
            int top = form.ClientSize.Height - 80;
            apply.Location = new Point(left, top);
            cancel.Location = new Point(left + 130 + 80, top);
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
                button.DialogResult = Enum.Parse<DialogResult>(names[i]);
                button.Size = new Size(110, 42);
                button.Location = new Point(270 - names.Length * 75 + i * 150, 180);
                form.Controls.Add(button);

            }
            FrmEditMaster.StyleDialog(form, 250);
            DialogResult result = form.ShowDialog();
            form.Dispose();
            return result;

        }
        public static DialogResult Show(IWin32Window owner, string text, string title = "Medicore Sound Designner",
        MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return Show(text, title, buttons, icon);
        }
  }

}

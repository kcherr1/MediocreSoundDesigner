namespace MediocreSoundDesigner
{
  partial class FrmGenerateChord
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenerateChord));
      nudStartTime = new NumericUpDown();
      lblStartTime = new Label();
      btnCancel = new Button();
      btnCreate = new Button();
      nudDuration = new NumericUpDown();
      lblDuration = new Label();
      nudOctave = new NumericUpDown();
      lblFrequency = new Label();
      cmbRootnote = new ComboBox();
      lblRootnote = new Label();
      cmbChordType = new ComboBox();
      lblChordType = new Label();
      ((System.ComponentModel.ISupportInitialize)nudStartTime).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).BeginInit();
      ((System.ComponentModel.ISupportInitialize)nudOctave).BeginInit();
      SuspendLayout();
      // 
      // nudStartTime
      // 
      nudStartTime.BackColor = Color.DimGray;
      nudStartTime.DecimalPlaces = 1;
      resources.ApplyResources(nudStartTime, "nudStartTime");
      nudStartTime.ForeColor = Color.White;
      nudStartTime.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudStartTime.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
      nudStartTime.Name = "nudStartTime";
      // 
      // lblStartTime
      // 
      lblStartTime.BackColor = Color.Transparent;
      resources.ApplyResources(lblStartTime, "lblStartTime");
      lblStartTime.Name = "lblStartTime";
      // 
      // btnCancel
      // 
      btnCancel.BackColor = Color.DimGray;
      btnCancel.DialogResult = DialogResult.Cancel;
      resources.ApplyResources(btnCancel, "btnCancel");
      btnCancel.Name = "btnCancel";
      btnCancel.UseVisualStyleBackColor = false;
      // 
      // btnCreate
      // 
      btnCreate.BackColor = Color.DimGray;
      btnCreate.DialogResult = DialogResult.OK;
      resources.ApplyResources(btnCreate, "btnCreate");
      btnCreate.Name = "btnCreate";
      btnCreate.UseVisualStyleBackColor = false;
      // 
      // nudDuration
      // 
      nudDuration.BackColor = Color.DimGray;
      nudDuration.DecimalPlaces = 1;
      resources.ApplyResources(nudDuration, "nudDuration");
      nudDuration.ForeColor = Color.White;
      nudDuration.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
      nudDuration.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
      nudDuration.Name = "nudDuration";
      nudDuration.Value = new decimal(new int[] { 2, 0, 0, 0 });
      // 
      // lblDuration
      // 
      lblDuration.BackColor = Color.Transparent;
      resources.ApplyResources(lblDuration, "lblDuration");
      lblDuration.Name = "lblDuration";
      // 
      // nudOctave
      // 
      nudOctave.BackColor = Color.DimGray;
      resources.ApplyResources(nudOctave, "nudOctave");
      nudOctave.ForeColor = Color.White;
      nudOctave.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
      nudOctave.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      nudOctave.Name = "nudOctave";
      nudOctave.Value = new decimal(new int[] { 4, 0, 0, 0 });
      // 
      // lblFrequency
      // 
      lblFrequency.BackColor = Color.Transparent;
      resources.ApplyResources(lblFrequency, "lblFrequency");
      lblFrequency.Name = "lblFrequency";
      // 
      // cmbRootnote
      // 
      cmbRootnote.BackColor = Color.DimGray;
      cmbRootnote.DropDownStyle = ComboBoxStyle.DropDownList;
      resources.ApplyResources(cmbRootnote, "cmbRootnote");
      cmbRootnote.ForeColor = Color.White;
      cmbRootnote.FormattingEnabled = true;
      cmbRootnote.Items.AddRange(new object[] { resources.GetString("cmbRootnote.Items"), resources.GetString("cmbRootnote.Items1"), resources.GetString("cmbRootnote.Items2"), resources.GetString("cmbRootnote.Items3"), resources.GetString("cmbRootnote.Items4"), resources.GetString("cmbRootnote.Items5"), resources.GetString("cmbRootnote.Items6"), resources.GetString("cmbRootnote.Items7"), resources.GetString("cmbRootnote.Items8"), resources.GetString("cmbRootnote.Items9"), resources.GetString("cmbRootnote.Items10"), resources.GetString("cmbRootnote.Items11") });
      cmbRootnote.Name = "cmbRootnote";
      cmbRootnote.SelectedIndexChanged += cmbRootnote_SelectedIndexChanged;
      // 
      // lblRootnote
      // 
      lblRootnote.BackColor = Color.Transparent;
      resources.ApplyResources(lblRootnote, "lblRootnote");
      lblRootnote.Name = "lblRootnote";
      // 
      // cmbChordType
      // 
      cmbChordType.BackColor = Color.DimGray;
      cmbChordType.DropDownStyle = ComboBoxStyle.DropDownList;
      resources.ApplyResources(cmbChordType, "cmbChordType");
      cmbChordType.ForeColor = Color.White;
      cmbChordType.FormattingEnabled = true;
      cmbChordType.Items.AddRange(new object[] { resources.GetString("cmbChordType.Items"), resources.GetString("cmbChordType.Items1"), resources.GetString("cmbChordType.Items2"), resources.GetString("cmbChordType.Items3") });
      cmbChordType.Name = "cmbChordType";
      // 
      // lblChordType
      // 
      lblChordType.BackColor = Color.Transparent;
      resources.ApplyResources(lblChordType, "lblChordType");
      lblChordType.Name = "lblChordType";
      lblChordType.Click += lblChordType_Click;
      // 
      // FrmGenerateChord
      // 
      resources.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      Controls.Add(cmbChordType);
      Controls.Add(lblChordType);
      Controls.Add(nudStartTime);
      Controls.Add(lblStartTime);
      Controls.Add(btnCancel);
      Controls.Add(btnCreate);
      Controls.Add(nudDuration);
      Controls.Add(lblDuration);
      Controls.Add(nudOctave);
      Controls.Add(lblFrequency);
      Controls.Add(cmbRootnote);
      Controls.Add(lblRootnote);
      ForeColor = Color.White;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmGenerateChord";
      ShowIcon = false;
      Load += FrmGenerateChord_Load;
      ((System.ComponentModel.ISupportInitialize)nudStartTime).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudDuration).EndInit();
      ((System.ComponentModel.ISupportInitialize)nudOctave).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private NumericUpDown nudStartTime;
    private Label lblStartTime;
    private Button btnCancel;
    private Button btnCreate;
    private NumericUpDown nudDuration;
    private Label lblDuration;
    private NumericUpDown nudOctave;
    private Label lblFrequency;
    private ComboBox cmbRootnote;
    private Label lblRootnote;
    private ComboBox cmbChordType;
    private Label lblChordType;
  }
}
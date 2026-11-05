using System.Text.Json;
using System.Text.Json.Serialization;
using Color = System.Drawing.Color;

namespace MediocreSoundDesigner;

public class ColorJsonConverter : JsonConverter<Color> {
  public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => ColorTranslator.FromHtml(reader.GetString());
  public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) => writer.WriteStringValue("#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2").ToLower());
}

public static class Settings {
  public class Colors {
    public Color WinBg { get; set; }
    public Color WinFg { get; set; }
    public Color TrackSamples_Enabled { get; set; }
    public Color TrackSamples_Disabled { get; set; }
    public Color TrackBg_Unselected { get; set; }
    public Color TrackBg_Selected { get; set; }
    public Color Playline { get; set; }
    public Color TrackHeader_Bg { get; set; }
    public Color TrackHeader_Selected { get; set; }
    public Color TrackHeader_Text { get; set; }
    public Color MuteDot_On { get; set; }
    public Color MuteDot_Off { get; set; }
    public Color TopBar_Bg { get; set; }
    public Color TopBar_Border { get; set; }

    public Colors() {
      WinBg = System.Drawing.Color.Black;
      WinFg = System.Drawing.Color.White;
      TrackSamples_Enabled = System.Drawing.Color.Cyan;
      TrackSamples_Disabled = System.Drawing.Color.LightGray;
      TrackBg_Unselected = System.Drawing.Color.DimGray;
      TrackBg_Selected = System.Drawing.Color.Gray;
      Playline = System.Drawing.Color.White;
      TrackHeader_Bg = System.Drawing.Color.FromArgb(58, 66, 77);
      TrackHeader_Selected = System.Drawing.Color.FromArgb(84, 96, 112);
      TrackHeader_Text = System.Drawing.Color.FromArgb(230, 232, 235);
      MuteDot_On = System.Drawing.Color.FromArgb(158, 232, 36);
      MuteDot_Off = System.Drawing.Color.FromArgb(95, 100, 106);
      TopBar_Bg = System.Drawing.Color.FromArgb(40, 46, 54);
      TopBar_Border = System.Drawing.Color.FromArgb(74, 84, 96);
    }
  }

  static Settings() {
    LoadFromFile();
  }

  public static Colors Color { get; private set; }

  private const string FILENAME = "settings.json";

  public static void SaveToFile() {
    JsonSerializerOptions options = new() { Converters = { new ColorJsonConverter() } };
    File.WriteAllText(FILENAME, JsonSerializer.Serialize(Color, options));
  }

  public static void LoadFromFile() {
    if (File.Exists(FILENAME)) {
      JsonSerializerOptions options = new() { Converters = { new ColorJsonConverter() } };
      Color = JsonSerializer.Deserialize<Colors>(File.ReadAllText(FILENAME), options);
    }
    else {
      Color = new();
    }
  }

  public static void RevertToDefault() {
    Color = new();
  }
}

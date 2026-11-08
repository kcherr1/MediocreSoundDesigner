using System.Text.Json;
using System.Text.Json.Serialization;
using Color = System.Drawing.Color;

namespace MediocreSoundDesigner;

public class ColorJsonConverter : JsonConverter<Color> {
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => ColorTranslator.FromHtml(reader.GetString());
    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) => writer.WriteStringValue("#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2").ToLower());
}

public static class Settings {
    private sealed class PersistedSettings
    {
        public Colors Color { get; set; } = new();
        public int AudioInputDeviceNumber { get; set; }
        public int AudioOutputDeviceNumber { get; set; }
    }

    public class Colors
    {
        public Color WinBg { get; set; }
        public Color WinFg { get; set; }
        public Color TrackSamples_Enabled { get; set; }
        public Color TrackSamples_Disabled { get; set; }
        public Color TrackBg_Unselected { get; set; }
        public Color TrackBg_Selected { get; set; }
        public Color Playline { get; set; }
        public Color TrackSelection { get; set; }

        public Colors()
        {
            WinBg = System.Drawing.Color.Black;
            WinFg = System.Drawing.Color.White;
            TrackSamples_Enabled = System.Drawing.Color.Cyan;
            TrackSamples_Disabled = System.Drawing.Color.LightGray;
            TrackBg_Unselected = System.Drawing.Color.DimGray;
            TrackBg_Selected = System.Drawing.Color.Gray;
            Playline = System.Drawing.Color.White;
            TrackSelection = System.Drawing.Color.FromArgb(90, 0, 150, 255);
        }
    }

    static Settings() {
    LoadFromFile();
    }

    public static Colors Color { get; private set; }
    public static int AudioInputDeviceNumber { get; set; }
    public static int AudioOutputDeviceNumber { get; set; }

    private const string FILENAME = "settings.json";

    public static void SaveToFile() {
        JsonSerializerOptions options = new() { Converters = { new ColorJsonConverter() }, WriteIndented = true };
        PersistedSettings settings = new()
        {
            Color = Color,
            AudioInputDeviceNumber = AudioInputDeviceNumber,
            AudioOutputDeviceNumber = AudioOutputDeviceNumber
        };
        File.WriteAllText(FILENAME, JsonSerializer.Serialize(settings, options));
    }

    public static void LoadFromFile() {
        Color = new();
        AudioInputDeviceNumber = 0;
        AudioOutputDeviceNumber = 0;

        if (!File.Exists(FILENAME)) return;

        try
        {
            string json = File.ReadAllText(FILENAME);
            if (string.IsNullOrWhiteSpace(json)) return;

            JsonSerializerOptions options = new() { Converters = { new ColorJsonConverter() } };
            using JsonDocument document = JsonDocument.Parse(json);

            if (document.RootElement.TryGetProperty("Color", out _))
            {
                PersistedSettings settings = JsonSerializer.Deserialize<PersistedSettings>(json, options);
                Color = settings?.Color ?? new();
                AudioInputDeviceNumber = settings?.AudioInputDeviceNumber ?? 0;
                AudioOutputDeviceNumber = settings?.AudioOutputDeviceNumber ?? 0;
            }
            else
            {
                Color = JsonSerializer.Deserialize<Colors>(json, options) ?? new();
            }
        }
        catch (JsonException)
        {
            Color = new();
            AudioInputDeviceNumber = 0;
            AudioOutputDeviceNumber = 0;
        }
    }

    public static void RevertToDefault() {
        Color = new();
        AudioInputDeviceNumber = 0;
        AudioOutputDeviceNumber = 0;
    }
}

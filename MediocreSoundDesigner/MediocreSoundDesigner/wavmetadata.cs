using System.Text;

namespace MediocreSoundDesigner;
/// <summary>
/// Descriptive tags (title, artist, ...) that get embedded in an exported WAV file
/// as a standard RIFF LIST/INFO chunk.
/// </summary>
public class WavMetadata
{
    /// <summary>
    /// Name of the song
    /// </summary>
    public string Title { get; set; } = "";
    /// <summary>
    /// Name of the creator
    /// </summary>
    public string Artist { get; set; } = "";
    /// <summary>
    /// Album or project name
    /// </summary>
    public string Album { get; set; } = "";
    /// <summary>
    /// Year of creation, e.g. "2026"
    /// </summary>
    public string Year { get; set; } = "";
    /// <summary>
    /// Free-form comment
    /// </summary>
    public string Comment { get; set; } = "";

    /// <summary>
    /// True when every field is blank, meaning there is nothing to embed
    /// </summary>
    public bool IsEmpty =>
      string.IsNullOrWhiteSpace(Title) &&
      string.IsNullOrWhiteSpace(Artist) &&
      string.IsNullOrWhiteSpace(Album) &&
      string.IsNullOrWhiteSpace(Year) &&
      string.IsNullOrWhiteSpace(Comment);

    /// <summary>
    /// Appends the tags to a finished WAV file and fixes the RIFF size in the header.
    /// Call this after the file has been fully written and closed.
    /// </summary>
    /// <param name="path">Path of the WAV file to tag</param>
    public void AppendTo(string path)
    {
        if (IsEmpty) return;

        // Body of the LIST chunk: "INFO" followed by one sub-chunk per field
        using MemoryStream body = new();
        body.Write(Encoding.ASCII.GetBytes("INFO"));
        WriteField(body, "INAM", Title);
        WriteField(body, "IART", Artist);
        WriteField(body, "IPRD", Album);
        WriteField(body, "ICRD", Year);
        WriteField(body, "ICMT", Comment);
        WriteField(body, "ISFT", "Mediocre Sound Designer");

        using FileStream fs = new(path, FileMode.Open, FileAccess.ReadWrite);
        fs.Seek(0, SeekOrigin.End);
        if (fs.Length % 2 == 1) fs.WriteByte(0);   // RIFF chunks start on even offsets
        fs.Write(Encoding.ASCII.GetBytes("LIST"));
        fs.Write(BitConverter.GetBytes((uint)body.Length));
        body.WriteTo(fs);

        // RIFF size (bytes 4-7) is the total file length minus the 8-byte RIFF header
        fs.Seek(4, SeekOrigin.Begin);
        fs.Write(BitConverter.GetBytes((uint)(fs.Length - 8)));
    }

    private static void WriteField(MemoryStream stream, string id, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        byte[] text = Encoding.UTF8.GetBytes(value.Trim() + "\0");   // INFO strings are null-terminated
        stream.Write(Encoding.ASCII.GetBytes(id));
        stream.Write(BitConverter.GetBytes((uint)text.Length));
        stream.Write(text);
        if (text.Length % 2 == 1) stream.WriteByte(0);              // pad to even length
    }
}

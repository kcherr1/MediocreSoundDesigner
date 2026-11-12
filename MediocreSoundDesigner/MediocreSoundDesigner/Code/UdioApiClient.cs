using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MediocreSoundDesigner;

public sealed class UdioApiClient : IDisposable
{
    private const string DefaultBaseAddress = "https://udioapi.pro/";
    private readonly HttpClient httpClient;
    private readonly bool ownsHttpClient;

    public UdioApiClient(string apiKey = null, HttpClient httpClient = null)
    {
        apiKey ??= LoadApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("The Udio API key was not found. Set API_KEY in .env or as an environment variable.");
        }

        this.httpClient = httpClient ?? new HttpClient();
        ownsHttpClient = httpClient is null;

        if (this.httpClient.BaseAddress is null)
        {
            this.httpClient.BaseAddress = new Uri(DefaultBaseAddress);
        }

        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey.Trim());
    }

    public async Task<JsonDocument> GenerateAsync(
        string prompt,
        bool makeInstrumental,
        CancellationToken cancellationToken = default)
    {
        string model = "chirp-v6-mini";
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("A generation prompt is required.", nameof(prompt));
        }

        var request = new
        {
            model,
            gpt_description_prompt = prompt,
            make_instrumental = makeInstrumental
        };

        return await SendJsonAsync(
            HttpMethod.Post,
            "api/v2/generate",
            JsonSerializer.Serialize(request),
            cancellationToken);
    }

    public Task<JsonDocument> GetFeedAsync(string workId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(workId))
        {
            throw new ArgumentException("A work ID is required.", nameof(workId));
        }

        string encodedWorkId = Uri.EscapeDataString(workId);
        return SendJsonAsync(HttpMethod.Get, $"api/v2/feed?workId={encodedWorkId}", null, cancellationToken);
    }

    public async Task<string> GenerateAndDownloadAsync(
        string prompt,
        bool makeInstrumental = true,
        TimeSpan? pollInterval = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        using JsonDocument generation = await GenerateAsync(prompt, makeInstrumental, cancellationToken);
        if (!TryGetWorkId(generation.RootElement, out string workId))
        {
            throw new InvalidOperationException("UdioAPI did not return a work ID for the generation request.");
        }

        TimeSpan interval = pollInterval ?? TimeSpan.FromSeconds(5);
        DateTime deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromMinutes(10));

        while (DateTime.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using JsonDocument feed = await GetFeedAsync(workId, cancellationToken);

            if (TryGetAudioUrl(feed.RootElement, out Uri audioUri))
            {
                return await DownloadAudioAsync(audioUri, cancellationToken);
            }

            if (IsFailed(feed.RootElement, out string failure))
            {
                throw new InvalidOperationException($"UdioAPI generation failed{(string.IsNullOrWhiteSpace(failure) ? "." : $": {failure}")}");
            }

            await Task.Delay(interval, cancellationToken);
        }

        throw new TimeoutException($"UdioAPI did not finish generating work ID '{workId}' before the timeout.");
    }

    public static bool TryGetWorkId(JsonElement response, out string workId)
    {
        workId = null;
        return TryFindProperty(response, "workId", out workId);
    }

    public void Dispose()
    {
        if (ownsHttpClient)
        {
            httpClient.Dispose();
        }
    }

    private async Task<JsonDocument> SendJsonAsync(
        HttpMethod method,
        string relativeUrl,
        string jsonBody,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, relativeUrl);
        if (jsonBody is not null)
        {
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        }

        using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"UdioAPI request failed with {(int)response.StatusCode} ({response.ReasonPhrase}): {responseBody}");
        }

        try
        {
            return JsonDocument.Parse(responseBody);
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException("UdioAPI returned invalid JSON.", exception);
        }
    }

    private static bool TryFindProperty(JsonElement element, string propertyName, out string value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase) &&
                    property.Value.ValueKind == JsonValueKind.String)
                {
                    value = property.Value.GetString();
                    return !string.IsNullOrWhiteSpace(value);
                }

                if (TryFindProperty(property.Value, propertyName, out value))
                {
                    return true;
                }
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in element.EnumerateArray())
            {
                if (TryFindProperty(item, propertyName, out value))
                {
                    return true;
                }
            }
        }

        value = null;
        return false;
    }

    private static bool TryGetAudioUrl(JsonElement response, out Uri audioUri)
    {
        string[] propertyNames =
        {
            "audio_url", "audioUrl", "download_url", "downloadUrl",
            "song_url", "songUrl", "stream_url", "streamUrl", "audio", "url"
        };

        foreach (string propertyName in propertyNames)
        {
            if (TryFindProperty(response, propertyName, out string value) &&
                Uri.TryCreate(value, UriKind.Absolute, out audioUri) &&
                (audioUri.Scheme == Uri.UriSchemeHttp || audioUri.Scheme == Uri.UriSchemeHttps))
            {
                return true;
            }
        }

        audioUri = null;
        return false;
    }

    private static bool IsFailed(JsonElement response, out string failure)
    {
        foreach (string propertyName in new[] { "status", "state" })
        {
            if (!TryFindProperty(response, propertyName, out string value)) continue;

            if (value.Equals("failed", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("error", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("cancelled", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("canceled", StringComparison.OrdinalIgnoreCase))
            {
                failure = value;
                return true;
            }
        }

        failure = null;
        return false;
    }

    private async Task<string> DownloadAudioAsync(Uri audioUri, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(
            audioUri,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        string extension = Path.GetExtension(audioUri.AbsolutePath);
        if (extension is not ".wav" and not ".mp3" and not ".flac" and not ".m4a")
        {
            extension = response.Content.Headers.ContentType?.MediaType switch
            {
                "audio/wav" or "audio/x-wav" => ".wav",
                "audio/mp4" or "audio/x-m4a" => ".m4a",
                "audio/flac" => ".flac",
                _ => ".mp3"
            };
        }

        string directory = Path.Combine(GetProjectDirectory(), "Generated Songs");
        Directory.CreateDirectory(directory);
        string filePath = Path.Combine(directory, $"udio-{Guid.NewGuid():N}{extension}");

        await using Stream source = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using FileStream destination = File.Create(filePath);
        await source.CopyToAsync(destination, cancellationToken);
        return filePath;
    }

    private static string GetProjectDirectory()
    {
        foreach (string directory in GetSearchDirectories())
        {
            if (File.Exists(Path.Combine(directory, "MediocreSoundDesigner.csproj")))
            {
                return directory;
            }
        }

        return AppContext.BaseDirectory;
    }

    private static string LoadApiKey()
    {
        string environmentKey = Environment.GetEnvironmentVariable("API_KEY");
        if (!string.IsNullOrWhiteSpace(environmentKey))
        {
            return environmentKey;
        }

        foreach (string directory in GetSearchDirectories())
        {
            string envPath = Path.Combine(directory, ".env");
            if (!File.Exists(envPath)) continue;

            foreach (string line in File.ReadLines(envPath))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;

                int separator = trimmed.IndexOf('=');
                if (separator <= 0 || !string.Equals(trimmed[..separator].Trim(), "API_KEY", StringComparison.Ordinal)) continue;

                return trimmed[(separator + 1)..].Trim().Trim('"', '\'');
            }
        }

        return null;
    }

    private static IEnumerable<string> GetSearchDirectories()
    {
        HashSet<string> visited = new(StringComparer.OrdinalIgnoreCase);
        foreach (string startingDirectory in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            DirectoryInfo directory = new(startingDirectory);
            while (directory is not null && visited.Add(directory.FullName))
            {
                yield return directory.FullName;
                directory = directory.Parent;
            }
        }
    }
}

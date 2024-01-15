using System.Text.Json.Serialization;

namespace DidacticalEnigma.LibreTranslate;

public record LibreTranslateQuery(
    [property: JsonPropertyName("q")] string Query,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("target")] string Target,
    [property: JsonPropertyName("format")] string? Format = null,
    [property: JsonPropertyName("api_key")] string? ApiKey = null);
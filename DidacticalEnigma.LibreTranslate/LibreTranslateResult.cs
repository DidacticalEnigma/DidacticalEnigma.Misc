using System.Text.Json.Serialization;

namespace DidacticalEnigma.LibreTranslate;

public record LibreTranslateResult(
    [property: JsonPropertyName("translatedText")] string TranslatedText);
using System.Text.Json.Serialization;

namespace DidacticalEnigma.SugoiTranslatorOfflineClient;

public class TranslationRequest
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
using System.Text.Json.Serialization;

namespace DidacticalEnigma.OCRAggregatorClient;

internal class DetectedTextJson
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    
    [JsonPropertyName("rect")]
    public IReadOnlyList<int> Rect { get; set; }
}
using System.Text.Json.Serialization;

namespace DidacticalEnigma.SugoiSuite;

internal class Request
{
    [JsonPropertyName("message")]
    public string Message { get; }
    
    [JsonPropertyName("content")]
    public string Content { get; }

    public Request(string message, string content)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}
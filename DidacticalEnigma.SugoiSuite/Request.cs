using System.Text.Json.Serialization;

namespace DidacticalEnigma.SugoiSuite;

internal class Request<T>
{
    [JsonPropertyName("message")]
    public string Message { get; }
    
    [JsonPropertyName("content")]
    public T Content { get; }

    public Request(string message, T content)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}
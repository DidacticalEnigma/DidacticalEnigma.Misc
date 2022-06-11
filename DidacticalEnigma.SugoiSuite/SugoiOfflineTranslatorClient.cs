using System.Net.Http.Json;
using System.Text.Json;

namespace DidacticalEnigma.SugoiSuite;

public class SugoiOfflineTranslatorClient
{
    private readonly HttpClient client;

    public SugoiOfflineTranslatorClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<string> GetTranslation(string source)
    {
        var response = await this.client.PostAsJsonAsync("/", new Request<string>(
            message: "translate sentences",
            content: source));
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStreamAsync())
            ?? throw new JsonException();
        return result;
    }
    
    public async Task<IEnumerable<KeyValuePair<string, string>>> GetTranslation(IEnumerable<string> source)
    {
        var inputSentences = source.ToArray();
        var response = await this.client.PostAsJsonAsync("/", new Request<string[]>(
            message: "translate sentences",
            content: inputSentences));
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<string[]>(await response.Content.ReadAsStreamAsync())
                     ?? throw new JsonException();
        return inputSentences.Zip(result, KeyValuePair.Create);
    }
}
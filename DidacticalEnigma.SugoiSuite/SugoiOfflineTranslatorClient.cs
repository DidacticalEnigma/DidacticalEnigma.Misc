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
        var response = await this.client.PostAsJsonAsync("/", new Request(
            message: "translate sentences",
            content: source));
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStreamAsync())
            ?? throw new JsonException();
        return result;
    }
}
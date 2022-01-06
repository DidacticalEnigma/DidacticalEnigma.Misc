using System.Net.Http.Json;
using System.Text.Json;

namespace DidacticalEnigma.SugoiTranslatorOfflineClient;

public class TranslationClient
{
    private readonly HttpClient client;

    public TranslationClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<string> GetTranslation(string source)
    {
        var response = await this.client.PostAsJsonAsync("/", new TranslationRequest()
        {
            Message = "translate sentences",
            Content = source
        });
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStreamAsync())
            ?? throw new JsonException();
        return result;
    }
}
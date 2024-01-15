using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DidacticalEnigma.LibreTranslate;

public class LibreTranslateClient
{
    private readonly HttpClient httpClient;

    public LibreTranslateClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<LibreTranslateResult> Translate(LibreTranslateQuery query)
    {
        var response = await this.httpClient.PostAsJsonAsync("/translate", query);
        
        while (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        response.EnsureSuccessStatusCode();

        var result = await JsonSerializer.DeserializeAsync<LibreTranslateResult>(
            await response.Content.ReadAsStreamAsync())
            ?? throw new InvalidDataException();

        return result;
    }
}
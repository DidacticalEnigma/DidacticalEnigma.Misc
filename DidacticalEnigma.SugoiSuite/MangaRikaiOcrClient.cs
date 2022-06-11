using System.Net.Http.Json;
using System.Text.Json;

namespace DidacticalEnigma.SugoiSuite;

public class MangaRikaiOcrClient
{
    private readonly HttpClient client;
    private readonly string pathToMangaRikaiOcr;
    private SemaphoreSlim locker = new SemaphoreSlim(1, 1);

    public MangaRikaiOcrClient(HttpClient client, string pathToMangaRikaiOcr)
    {
        this.client = client;
        this.pathToMangaRikaiOcr = pathToMangaRikaiOcr;
    }

    public async Task<IEnumerable<TextRectangle>> DetectTextBoxes(Stream pngFile)
    {
        try
        {
            await this.locker.WaitAsync();
            await using (var targetFile = File.Create(Path.Combine(this.pathToMangaRikaiOcr, "backendServer", "wholeImage.png")))
            {
                await pngFile.CopyToAsync(targetFile);
            }

            var response = await this.client.PostAsJsonAsync("/", new Request<string>(
                message: "detect all textboxes",
                content: "no content"));
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<int[][]>(await response.Content.ReadAsStreamAsync())
                         ?? throw new JsonException();
            return result.Select(rectCoordinates =>
                new TextRectangle(
                    new Point2D(rectCoordinates[0], rectCoordinates[1]),
                    new Point2D(rectCoordinates[2], rectCoordinates[3])));
        }
        finally
        {
            this.locker.Release();
        }
    }
}
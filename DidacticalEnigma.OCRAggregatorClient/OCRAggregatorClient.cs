using System.Drawing;
using System.Text.Json;

namespace DidacticalEnigma.OCRAggregatorClient;

public class OCRAggregatorClient
{
    private readonly HttpClient client;

    public OCRAggregatorClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IReadOnlyCollection<DetectedText>> DetectOcr(string pathToImageFile)
    {
        await using var file = File.OpenRead(pathToImageFile);
        return await DetectOcr(file);
    }

    public async Task<IReadOnlyCollection<DetectedText>> DetectOcr(Stream inputImage)
    {
        var url = "/detect_ocr";

        var content = new MultipartFormDataContent()
        {
            { new StreamContent(inputImage), "\"input_image\"", "blob" }
        };
        
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<IEnumerable<DetectedTextJson>>(await response.Content.ReadAsStreamAsync())
                     ?? throw new InvalidDataException();

        return result
            .Select(json => new DetectedText(
                text: json.Text,
                rectangle: Rectangle.FromLTRB(json.Rect[0], json.Rect[1], json.Rect[2], json.Rect[3])))
            .ToList();
    }

    public async Task<string> Ocr(string pathToImageFile)
    {
        await using var file = File.OpenRead(pathToImageFile);
        return await Ocr(file);
    }
    
    public async Task<string> Ocr(Stream inputImage)
    {
        var url = "/ocr";

        var content = new MultipartFormDataContent()
        {
            { new StreamContent(inputImage), "\"input_image\"", "blob" }
        };
        
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<string>(await response.Content.ReadAsStreamAsync())
                     ?? throw new InvalidDataException();

        return result;
    }
    
    public async Task<IEnumerable<Rectangle>> Detect(string pathToImageFile)
    {
        await using var file = File.OpenRead(pathToImageFile);
        return await Detect(file);
    }
    
    public async Task<IEnumerable<Rectangle>> Detect(Stream inputImage)
    {
        var url = "/detect";

        var content = new MultipartFormDataContent()
        {
            { new StreamContent(inputImage), "\"input_image\"", "blob" }
        };
        
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<IEnumerable<IReadOnlyList<int>>>(await response.Content.ReadAsStreamAsync())
                     ?? throw new InvalidDataException();

        return result
            .Select(json => Rectangle.FromLTRB(json[0], json[1], json[2], json[3]))
            .ToList();
    }
}
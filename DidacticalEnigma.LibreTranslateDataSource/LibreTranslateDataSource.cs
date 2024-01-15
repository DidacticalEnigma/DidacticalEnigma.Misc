using DidacticalEnigma.Core.Models.DataSources;
using DidacticalEnigma.Core.Models.Formatting;
using DidacticalEnigma.LibreTranslate;
using Optional;

namespace DidacticalEnigma.LibreTranslateDataSource;

public class LibreTranslateDataSource : IDataSource
{
    private readonly LibreTranslateClient client;

    public static DataSourceDescriptor Descriptor { get; } = new DataSourceDescriptor(
        new Guid("BFDD187F-BF59-4436-8F23-E5375188E2B2"),
        "LibreTranslate",
        "LibreTranslate is maintained by community at https://github.com/LibreTranslate/LibreTranslate",
        new Uri("https://libretranslate.com/"));

    private readonly LibreTranslateQuery query = new(
        Query: "example text",
        Source: "ja",
        Target: "en",
        Format: "text");

    private LruCache<string, RichFormatting> cache = new LruCache<string, RichFormatting>(64);

    public LibreTranslateDataSource(
        LibreTranslateClient client)
    {
        this.client = client;
    }
    
    public void Dispose()
    {
        
    }

    public async Task<Option<RichFormatting>> Answer(Request request, CancellationToken token)
    {
        var text = request.AllText().ReplaceLineEndings("");
        try
        {
            return (await cache.GetAsync(text, Translate(text, token), token)).Some();
        }
        catch (HttpRequestException ex)
        {
            var r = new RichFormatting();
            
            r.Paragraphs.Add(new TextParagraph(
                new Text[]
                {
                    new Text("ERROR: ", emphasis: true),
                    new Text("Cannot connect to LibreTranslate server."),
                }));
            
            r.Paragraphs.Add(new TextParagraph(
                new Text[]
                {
                    new Text("More details as follows:\n", emphasis: true),
                    new Text(ex.Message),
                }));

            return r.Some();
        }
    }

    private Func<Task<RichFormatting>> Translate(string text, CancellationToken token)
    {
        return async () =>
        {
            var translated = await this.client.Translate(query with
            {
                Query = text
            });

            return new RichFormatting()
            {
                Paragraphs =
                {
                    new TextParagraph()
                    {
                        Content =
                        {
                            new Text(translated.TranslatedText)
                        }
                    }
                }
            };
        };
    }

    public Task<UpdateResult> UpdateLocalDataSource(CancellationToken cancellation = new CancellationToken())
    {
        return Task.FromResult(UpdateResult.NotSupported);
    }

    public string? InstanceIdentifier => null;
}
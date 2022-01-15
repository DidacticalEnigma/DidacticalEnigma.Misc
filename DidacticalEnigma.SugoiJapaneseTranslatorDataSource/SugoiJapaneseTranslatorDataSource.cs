using DidacticalEnigma.Core.Models.DataSources;
using DidacticalEnigma.Core.Models.Formatting;
using DidacticalEnigma.SugoiSuite;
using Optional;
using TinyIndex;

namespace DidacticalEnigma.SugoiJapaneseTranslatorDataSource;

public class SugoiJapaneseTranslatorDataSource : IDataSource
{
    private readonly SugoiOfflineTranslatorClient client;

    public static DataSourceDescriptor Descriptor { get; } = new DataSourceDescriptor(
        new Guid("7AF17241-41A6-4B06-8A83-CEB16783FA0A"),
        "Sugoi Japanese Translator",
        "Sugoi Japanese Translator was created by MingShiba",
        new Uri("https://discord.gg/XFbWSjMHJh"));

    private LruCache<string, RichFormatting> cache = new LruCache<string, RichFormatting>(16);

    public SugoiJapaneseTranslatorDataSource(
        SugoiOfflineTranslatorClient client)
    {
        this.client = client;
    }
    
    public void Dispose()
    {
        
    }

    public async Task<Option<RichFormatting>> Answer(Request request, CancellationToken token)
    {
        var text = request.AllText();
        return (await cache.GetAsync(text, Translate(text, token), token)).Some();
    }

    private Func<Task<RichFormatting>> Translate(string text, CancellationToken token)
    {
        return async () =>
        {
            var translated = await this.client.GetTranslation(text);

            return new RichFormatting()
            {
                Paragraphs =
                {
                    new TextParagraph()
                    {
                        Content =
                        {
                            new Text(translated)
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
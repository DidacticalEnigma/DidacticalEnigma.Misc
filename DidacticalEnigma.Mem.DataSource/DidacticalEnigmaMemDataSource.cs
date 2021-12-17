using DidacticalEnigma.Core.Models.DataSources;
using DidacticalEnigma.Core.Models.Formatting;
using DidacticalEnigma.Mem.Client.MemApi;
using DidacticalEnigma.Mem.Client.MemApi.Models;
using Optional;

namespace DidacticalEnigma.Mem.DataSource;

public class DidacticalEnigmaMemDataSource : IDataSource
{
    private readonly Func<IDidacticalEnigmaMem?> memApiAccessor;
    
    public static DataSourceDescriptor Descriptor { get; } = new DataSourceDescriptor(
        new Guid("9A9BA935-6F8E-4319-ADB2-6D708E90A627"),
        "DidacticalEnigma.Mem Translation Memory Server",
        "",
        null);

    public DidacticalEnigmaMemDataSource(Func<IDidacticalEnigmaMem?> memApiAccessor)
    {
        this.memApiAccessor = memApiAccessor;
    }
    
    public void Dispose()
    {
        
    }

    public async Task<Option<RichFormatting>> Answer(Request request, CancellationToken token)
    {
        var api = this.memApiAccessor();

        if (api == null)
        {
            return Option.Some(new RichFormatting(new Paragraph[]
                {
                    new TextParagraph(new Text[]
                    {
                        new Text("The address to the DidacticalEnigma.Mem server is not configured")
                    })
                }));
        }

        var response = await api.QueryWithHttpMessagesAsync(
            query: request.Word.RawWord,
            cancellationToken: token,
            translatedOnly: true);

        var rich = new RichFormatting();
        
        if (response.Response.IsSuccessStatusCode)
        {
            QueryTranslationsResult result = response.Body;
            foreach (var tl in result.Translations)
            {
                var highlights = MarkHighlights(tl.Source, tl.HighlighterSequence);
                var textParagraph = new TextParagraph(
                    highlights
                        .Select(t => new Text(t.fragment, emphasis: t.highlight)));
                textParagraph.Content.Add(new Text("\n"));
                textParagraph.Content.Add(new Text(tl.Target));
                rich.Paragraphs.Add(textParagraph);
            }
        }
        else
        {
            rich.Paragraphs.Add(
                new TextParagraph(
                    new Text[]
                    {
                        new Text($"Error: {response.Response.StatusCode}")
                    }));
        }

        return rich.Some();
    }

    public static IEnumerable<(string fragment, bool highlight)> MarkHighlights(string text, string? highlighter)
    {
        if (highlighter == null)
        {
            yield return (text, false);
            yield break;
        }

        bool isHighlighted = false;
        foreach (var part in text.Split(highlighter))
        {
            yield return (part, isHighlighted);
            isHighlighted = !isHighlighted;
        }
    }

    public async Task<UpdateResult> UpdateLocalDataSource(CancellationToken cancellation = new CancellationToken())
    {
        return UpdateResult.NotSupported;
    }

    public string InstanceIdentifier { get; }
}
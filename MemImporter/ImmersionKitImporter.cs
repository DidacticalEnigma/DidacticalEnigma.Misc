using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JDict.ImmersionKit;
using DidacticalEnigma.Mem.Client.MemApi;
using DidacticalEnigma.Mem.Client.MemApi.Models;
using Newtonsoft.Json;

namespace MemImporter
{
    public class ImmersionKitImporter
    {
        private const int BatchSize = 100;
        
        public static async Task Import(IDidacticalEnigmaMem api, string projectName, IReadOnlyList<string> args)
        {
            var deckDirectoryPath = args[0];
            var dataFilePath = Path.Combine(deckDirectoryPath, "data.json");
            
            await foreach (var batch in ChunkBy(StreamingDeserialize(dataFilePath), BatchSize))
            {
                await api.AddTranslationsAsync(
                    projectName,
                    new AddTranslationsParams()
                    {
                        Translations = batch
                            .Select(entry => new AddTranslationParams()
                            {
                                Source = entry.Sentence,
                                Target = entry.Translation,
                                CorrelationId = $"{entry.DeckName}, {entry.Id}"
                            })
                            .ToList()
                    });
            }
        }

        public static async IAsyncEnumerable<IEnumerable<T>> ChunkBy<T>(IAsyncEnumerable<T> input, int n)
        {
            var list = new List<T>();
            int i = 0;
            await foreach (var element in input)
            {
                list.Add(element);
                i++;
                if (i == n)
                {
                    yield return list;
                    list = new List<T>();
                    i = 0;
                }
            }
            if (i != 0)
                yield return list;
        }

        private static async IAsyncEnumerable<ImmersionKitFullDataEntry> StreamingDeserialize(string dataFilePath)
        {
            var serializer = new JsonSerializer();
            using var reader = File.OpenText(dataFilePath);
            using var jsonReader = new JsonTextReader(reader);
            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    yield return serializer.Deserialize<ImmersionKitFullDataEntry>(jsonReader);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MagicTranslatorProject.Context;
using MagicTranslatorProjectMemImporter.MemApi;
using MagicTranslatorProjectMemImporter.MemApi.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MemImporter
{
    public class MagicTranslatorProjectImporter
    {
        public static async Task Import(IDidacticalEnigmaMem api, string projectName, IReadOnlyList<string> args)
        {
            var projectPath = args[0];

            var imagePaths = new Dictionary<string, Guid>();
            
            if (MagicTranslatorProject.MagicTranslatorProject.Registration.TryOpen(projectPath, out var project, out var failureReason))
            {
                foreach (var volume in project.Root.Children)
                {
                    foreach (var chapter in volume.Children)
                    {
                        foreach (var page in chapter.Children.OfType<PageContext>())
                        {
                            var translations = new List<AddTranslationParams>();
                            foreach (var capture in page.Children)
                            {
                                bool isNewImage = imagePaths.TryAdd(page.PathToRaw, Guid.NewGuid());
                                if (isNewImage)
                                {
                                    await using var memoryStream = new MemoryStream();
                                    using var img = await Image.LoadAsync(page.PathToRaw);
                                    img.Mutate(i =>
                                    {
                                        i.Resize(480, 0);
                                    });
                                        
                                    await img.SaveAsPngAsync(memoryStream);
                                    await api.AddContextsAsync(new AddContextsParams()
                                    {
                                        Contexts = new List<AddContextParams>()
                                        {
                                            new AddContextParams(imagePaths[page.PathToRaw], memoryStream.ToArray(),
                                                "image/png", page.ShortDescription)
                                        }
                                    });
                                }
                                translations.Add(new AddTranslationParams()
                                {
                                    Context = imagePaths[page.PathToRaw],
                                    CorrelationId = capture.ShortDescription + " " + Guid.NewGuid().ToString(),
                                    Source = capture.Translation.OriginalText,
                                    Target = capture.Translation.TranslatedText
                                });
                            }

                            await api.AddTranslationsAsync(projectName, new AddTranslationsParams()
                            {
                                Translations = translations
                            });
                        }
                    }
                }
            }
            else
            {
                await Console.Error.WriteLineAsync(failureReason);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DidacticalEnigma.Project;
using MagicTranslatorProject.Context;
using MagicTranslatorProjectMemImporter.MemApi;
using MagicTranslatorProjectMemImporter.MemApi.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MagicTranslatorProjectMemImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectPath = args[0];
            var projectName = args[1];
            var address = args[2];

            var imagePaths = new Dictionary<string, Guid>();
            
            var api = new DidacticalEnigmaMem(new Uri(address));

            if (MagicTranslatorProject.MagicTranslatorProject.Registration.TryOpen(projectPath, out var project, out var failureReason))
            {
                foreach (var volume in project.Root.Children)
                {
                    foreach (var chapter in volume.Children)
                    {
                        foreach (var page in chapter.Children.OfType<PageContext>())
                        {
                            var translations = new List<AddTranslation>();
                            foreach (var capture in page.Children)
                            {
                                bool isNewImage = imagePaths.TryAdd(page.PathToRaw, Guid.NewGuid());
                                if (isNewImage)
                                {
                                    using var memoryStream = new MemoryStream();
                                    using var img = Image.Load(page.PathToRaw);
                                    img.Mutate(i =>
                                    {
                                        i.Resize(480, 0);
                                    });
                                        
                                    img.SaveAsPng(memoryStream);
                                    api.Add(projectName, new AddTranslations()
                                    {
                                        Contexts = new List<AddContext>()
                                        {
                                            new AddContext(imagePaths[page.PathToRaw], memoryStream.ToArray(),
                                                "image/png", page.ShortDescription)
                                        }
                                    });
                                }
                                translations.Add(new AddTranslation()
                                {
                                    Context = imagePaths[page.PathToRaw],
                                    CorrelationId = capture.ShortDescription + " " + Guid.NewGuid().ToString(),
                                    Source = capture.Translation.OriginalText,
                                    Target = capture.Translation.TranslatedText
                                });
                            }

                            api.Add(projectName, new AddTranslations()
                            {
                                Translations = translations
                            });
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(failureReason);
            }
        }
    }
}
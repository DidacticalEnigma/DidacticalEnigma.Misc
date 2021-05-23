using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

            Console.WriteLine("Supply an access token (just the token, don't prefix it with \"Bearer\"):");
            var token = Console.ReadLine();

            var imagePaths = new Dictionary<string, Guid>();
            
            var api = new DidacticalEnigmaMem(new Uri(address));
            api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                                    using var memoryStream = new MemoryStream();
                                    using var img = Image.Load(page.PathToRaw);
                                    img.Mutate(i =>
                                    {
                                        i.Resize(480, 0);
                                    });
                                        
                                    img.SaveAsPng(memoryStream);
                                    api.AddContexts(new AddContextsParams()
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

                            api.AddTranslations(projectName, new AddTranslationsParams()
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
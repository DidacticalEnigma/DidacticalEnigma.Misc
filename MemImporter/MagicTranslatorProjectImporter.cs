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

            if (MagicTranslatorProject.MagicTranslatorProject.Registration.TryOpen(projectPath, out var project, out var failureReason))
            {
                foreach (var volume in project.Root.Children)
                {
                    foreach (var chapter in volume.Children)
                    {
                        foreach (var page in chapter.Children.OfType<PageContext>())
                        {
                            await using var file = File.OpenRead(page.PathToRaw);
                            await api.AddContextAsync(
                                Guid.NewGuid().ToString(),
                                projectName,
                                "image/png",
                                page.ShortDescription,
                                file,
                                null);
                            
                            var translations = new List<AddTranslationParams>();
                            foreach (var capture in page.Children)
                            {

                                translations.Add(new AddTranslationParams()
                                {
                                    CorrelationId = capture.ShortDescription + " " + Guid.NewGuid().ToString(),
                                    Source = capture.Translation.OriginalText,
                                    Target = capture.Translation.TranslatedText,
                                    TranslationNotes = new AddTranslationNotesParams(
                                        capture.Translation.Notes.Select(n => new IoNormalNote("", n.Text)).ToList(),
                                        capture.Translation.Glosses.Select(n => new IoGlossNote(n.Foreign, n.Text)).ToList())
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
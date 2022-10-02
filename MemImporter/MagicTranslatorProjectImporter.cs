using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using MagicTranslatorProject.Context;
using DidacticalEnigma.Mem.Client.MemApi;
using DidacticalEnigma.Mem.Client.MemApi.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MemImporter
{
    public class MagicTranslatorProjectImporter
    {
        public static async Task Import(IDidacticalEnigmaMem api, string projectName, IReadOnlyList<string> args)
        {
            var projectPath = args[0];
            var isInitRaw = args.ElementAtOrDefault(1);
            var identifer = args.ElementAtOrDefault(2);
            bool isInit = bool.Parse(isInitRaw);
            var subcomponentsCount = identifer?.Split("/", StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
            if (subcomponentsCount != 1 && subcomponentsCount != 2)
            {
                throw new ArgumentException("identifier is invalid");
            }

            if (MagicTranslatorProject.MagicTranslatorProject.Registration.TryOpen(projectPath, out var rawProject, out var failureReason))
            {
                var project = (MagicTranslatorProject.MagicTranslatorProject)rawProject;

                Dictionary<CharacterType, Guid> characterDict;

                if (isInit)
                {
                    characterDict = project.Root.AllCharacters.ToDictionary(c => c, c => Guid.NewGuid());

                    await api.AddCategoriesAsync(projectName, new AddCategoriesParams()
                    {
                        Categories = characterDict
                            .Select(kvp => new AddCategoryParams()
                            {
                                Id = kvp.Value,
                                Name = kvp.Key.ToString()
                            })
                            .ToList()
                    });
                }
                else
                {
                    var categoriesResponse = await api.GetCategoriesWithHttpMessagesAsync(projectName);
                    characterDict = project.Root.AllCharacters
                        .Join(
                            categoriesResponse.Body.Categories,
                            ch => ch.ToString(),
                            cat => cat.Name,
                            (ch, cat) => KeyValuePair.Create(ch, cat.Id.Value))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }

                var translations = new List<AddTranslationParams>();
                foreach (var volume in project.Root.Children)
                {
                    if (subcomponentsCount == 1)
                    {
                        if (identifer != volume.ReadableIdentifier)
                        {
                            Console.WriteLine($"Ignoring volume to upload: {volume.ReadableIdentifier}");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"Matched volume to upload: {volume.ReadableIdentifier}");
                        }
                    }

                    foreach (var chapter in volume.Children)
                    {
                        if (subcomponentsCount == 2)
                        {
                            if (identifer != chapter.ReadableIdentifier)
                            {
                                Console.WriteLine($"Ignoring chapter to upload: {chapter.ReadableIdentifier}");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine($"Matched chapter to upload: {chapter.ReadableIdentifier}");
                            }
                        }

                        foreach (var page in chapter.Children)
                        {
                            await using var file = File.OpenRead(Path.Combine(projectPath, page.PathToRaw));
                            await api.AddContextAsync(
                                Guid.NewGuid().ToString(),
                                projectName,
                                "image/png",
                                page.ReadableIdentifier,
                                file,
                                null);
                            
                            
                            foreach (var capture in page.Children)
                            {
                                var normalizedNotes =
                                    capture.Translation.Notes
                                        .Where(n => !string.IsNullOrWhiteSpace(n.Text))
                                        .Select(n => new IoNormalNote("", n.Text))
                                        .ToList();
                                var normalizedGlosses = capture.Translation.Glosses
                                    .Where(n => !string.IsNullOrWhiteSpace(n.Foreign) || !string.IsNullOrWhiteSpace(n.Text))
                                    .Select(n => new IoGlossNote(n.Foreign, n.Text))
                                    .ToList();
                                var hasNotesOrGlosses = normalizedGlosses.Count != 0 || normalizedNotes.Count != 0;
                                translations.Add(new AddTranslationParams()
                                {
                                    CorrelationId = capture.ReadableIdentifier,
                                    Source = capture.Translation.OriginalText,
                                    Target = string.IsNullOrWhiteSpace(capture.Translation.TranslatedText) ? null : capture.Translation.TranslatedText,
                                    CategoryId = characterDict[capture.Character],
                                    TranslationNotes = hasNotesOrGlosses
                                        ? new AddTranslationNotesParams(
                                            capture.Translation.Notes.Select(n => new IoNormalNote(n.SideText, n.Text)).ToList(),
                                            capture.Translation.Glosses.Select(n => new IoGlossNote(n.Foreign, n.Text)).ToList())
                                        : null
                                });
                            }

                            if (translations.Count > 100)
                            {
                                await api.AddTranslationsAsync(projectName, new AddTranslationsParams()
                                {
                                    Translations = translations
                                });
                                translations.Clear();
                            }
                        }
                    }
                }
                if (translations.Count > 0)
                {
                    await api.AddTranslationsAsync(projectName, new AddTranslationsParams()
                    {
                        Translations = translations
                    });
                    translations.Clear();
                }
            }
            else
            {
                await Console.Error.WriteLineAsync(failureReason);
            }
        }
    }
}
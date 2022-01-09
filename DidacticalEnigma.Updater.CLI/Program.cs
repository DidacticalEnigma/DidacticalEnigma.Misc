using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DidacticalEnigma.Core.Models.LanguageService;
using JetBrains.Annotations;
using NMeCab;

namespace DidacticalEnigma.Updater.CLI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var dataDirectory = GetDataDirectory(args.ElementAtOrDefault(0));
            var httpClient = new HttpClient();

            using var mecab = new MeCabIpadic(new MeCabParam
            {
                DicDir = Path.Combine(dataDirectory, "mecab", "ipadic"),
                UseMemoryMappedFile = true
            });

            var updater1 = new JMDictUpdaterProcess(
                httpClient,
                "http://ftp.edrdg.org/pub/Nihongo/JMdict_e.gz",
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.cache"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz.new"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.new.cache"),
                Path.Combine(dataDirectory, "dictionaries", "jmdict_tested_schema.xml"));

            updater1.OnUpdateStatusChange += UpdaterStatusUpdate(updater1.Name);

            var updater2 = new JMNedictUpdaterProcess(
                httpClient,
                "http://ftp.edrdg.org/pub/Nihongo/JMnedict.xml.gz",
                Path.Combine(dataDirectory, "dictionaries", "JMnedict.xml.gz"),
                Path.Combine(dataDirectory, "dictionaries", "JMnedict.xml.cache"),
                Path.Combine(dataDirectory, "dictionaries", "JMnedict.xml.gz.new"),
                Path.Combine(dataDirectory, "dictionaries", "JMnedict.xml.new.cache"),
                Path.Combine(dataDirectory, "dictionaries", "jmnedict_tested_schema.xml"));

            updater2.OnUpdateStatusChange += UpdaterStatusUpdate(updater2.Name);

            
            var updater3 = new KanjiDictUpdaterProcess(
                httpClient,
                "http://ftp.edrdg.org/pub/Nihongo/kanjidic2.xml.gz",
                Path.Combine(dataDirectory, "character", "kanjidic2.xml.gz"),
                Path.Combine(dataDirectory, "character", "kanjidic2.xml.gz.new"));

            updater3.OnUpdateStatusChange += UpdaterStatusUpdate(updater3.Name);
            
            var updater4 = new TanakaUpdaterProcess(
                httpClient,
                "http://ftp.edrdg.org/pub/Nihongo/examples.utf.gz",
                Path.Combine(dataDirectory, "corpora", "examples.utf.gz"),
                Path.Combine(dataDirectory, "corpora", "examples.utf.gz.new"),
                Path.Combine(dataDirectory, "corpora", "tanaka.cache"),
                Path.Combine(dataDirectory, "corpora", "tanaka.cache.new"),
                mecab);

            updater4.OnUpdateStatusChange += UpdaterStatusUpdate(updater4.Name);

            var task1 = updater1.Execute();
            var task2 = updater2.Execute();
            var task3 = updater3.Execute();
            var task4 = updater4.Execute();

            await Task.WhenAll(task1, task2, task3, task4);
        }

        private static Action<UpdateStatus> UpdaterStatusUpdate(string name)
        {
            return status =>
            {
                switch (status)
                {
                    case UpdateStatus.DownloadingStatus downloadingStatus:
                        if (downloadingStatus.Percentage == null)
                        {
                            Console.WriteLine($"{name} - Downloading...");
                        }
                        else
                        {
                            Console.WriteLine($"{name} - Downloading: {downloadingStatus.Percentage.Value}");
                        }

                        break;
                    case UpdateStatus.FailureStatus failureStatus:
                        Console.WriteLine($"{name} - Failure: {failureStatus.Reason}");
                        if (failureStatus.LongMessage != null)
                        {
                            Console.WriteLine(failureStatus.LongMessage);
                        }
                        break;
                    case UpdateStatus.ProcessingStatus processingStatus:
                        Console.WriteLine($"{name} - Processing...");
                        break;
                    case UpdateStatus.ReadyToStartStatus readyToStartStatus:
                        Console.WriteLine($"{name} - Ready to start.");
                        break;
                    case UpdateStatus.SuccessStatus successStatus:
                        Console.WriteLine($"{name} - Success!");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status));
                }
            };
        }
        
        public static string GetDataDirectory([CanBeNull] string dataDir)
        {
            return string.IsNullOrWhiteSpace(dataDir)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data")
                : dataDir;
        }
    }
}
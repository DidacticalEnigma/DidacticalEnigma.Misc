using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JDict;

namespace DidacticalEnigma.Updater
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var dataDirectory = args.ElementAtOrDefault(0) ?? "/home/milleniumbug/dokumenty/PROJEKTY/InDevelopment/DidacticalEnigma/Data";

            var updater = new JMDictUpdaterProcess(
                new HttpClient(),
                "http://ftp.edrdg.org/pub/Nihongo/JMdict_e.gz",
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.cache"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz.new"),
                Path.Combine(dataDirectory, "dictionaries", "JMdict_e.new.cache"));

            updater.OnUpdateStatusChange += status =>
            {
                switch (status)
                {
                    case UpdateStatus.DownloadingStatus downloadingStatus:
                        if (downloadingStatus.Percentage == null)
                        {
                            Console.WriteLine("Downloading...");
                        }
                        else
                        {
                            Console.WriteLine($"Downloading: {downloadingStatus.Percentage.Value}");
                        }

                        break;
                    case UpdateStatus.FailureStatus failureStatus:
                        Console.WriteLine($"Failure: {failureStatus.Reason}");
                        break;
                    case UpdateStatus.ProcessingStatus processingStatus:
                        Console.WriteLine("Processing...");
                        break;
                    case UpdateStatus.ReadyToStartStatus readyToStartStatus:
                        Console.WriteLine("Ready to start.");
                        break;
                    case UpdateStatus.SuccessStatus successStatus:
                        Console.WriteLine("Success!");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status));
                }
            };
            
            await updater.Execute();
        }
    }
}
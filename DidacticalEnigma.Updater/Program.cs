using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DidacticalEnigma.Updater
{
    class Program
    {
        private static readonly string NamePattern = "[:A-Za-z_\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD][:A-Za-z_\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD.0-9\u00B7\u0300-\u036F\u203F-\u2040-]*";
        private static readonly string WhitespaceCharacterPattern = "[ \t\r\n]";

        private static readonly Regex EntityRegex = new Regex(
            "<!ENTITY" + 
            $"{WhitespaceCharacterPattern}+" +
            $"({NamePattern})" +
            $"{WhitespaceCharacterPattern}+" +
            "\"([^%&\"]*)\"" +
            $"{WhitespaceCharacterPattern}*" +
            ">");

        private static IReadOnlyList<string> XmlDeclarations(string path)
        {
            using (var file = File.OpenRead(path))
            using (var gzip = new GZipStream(file, CompressionMode.Decompress))
            {
                var directives = DtdUtils.GetDtdDirectives(gzip)
                    .Select(decl =>
                    {
                        var match = EntityRegex.Match(decl);
                        if (match.Success)
                        {
                            return $"<!ENTITY {match.Groups[1]} \"...\">";
                        }
                        else
                        {
                            return decl;
                        }
                    })
                    .OrderBy(x => x)
                    .ToList();

                return directives;
            }
        }
        
        public static (IEnumerable<string> oldEntries, IEnumerable<string> newEntries) Diff(
            IReadOnlyList<string> inputOld,
            IReadOnlyList<string> inputNew)
        {
            return (
                inputNew.Except(inputOld),
                inputOld.Except(inputNew)
            );
        }

        public static async Task Main(string[] args)
        {
            var dataDirectory = args.ElementAtOrDefault(0) ?? "/home/milleniumbug/dokumenty/PROJEKTY/InDevelopment/DidacticalEnigma/Data";

            if (false)
            {
                var httpClient = new HttpClient();
                await httpClient.GetToFileAsync("http://ftp.edrdg.org/pub/Nihongo/JMdict_e.gz",
                    Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz.new"));
            }

            var declarationsNew = XmlDeclarations(Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz.new"));
            var declarationsOld = XmlDeclarations(Path.Combine(dataDirectory, "dictionaries", "JMdict_e.gz"));

            var (oldEntries, newEntries) = Diff(declarationsOld, declarationsNew);
            bool hasAnyDifference = false;
            foreach (var oldEntry in oldEntries)
            {
                hasAnyDifference = true;
                Console.WriteLine($"- {oldEntry}");
            }

            foreach (var newEntry in newEntries)
            {
                hasAnyDifference = true;
                Console.WriteLine($"+ {newEntry}");
            }

            if (!hasAnyDifference)
            {
                Console.WriteLine("no diff, can safely update!");
            }
        }
    }
}
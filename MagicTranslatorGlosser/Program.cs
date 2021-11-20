using System;
using System.IO;
using System.Text;
using DidacticalEnigma.Core.Models;
using DidacticalEnigma.Core.Models.LanguageService;
using JDict;
using Newtonsoft.Json;
using NMeCab;

namespace MagicTranslatorGlosser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            RunAutomaticGlossing(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Data"), args[0]);
        }
        
        private static void RunAutomaticGlossing(string dataDir, string input)
        {
            var kana = new KanaProperties2(Path.Combine(dataDir, "character", "kana.txt"), Encoding.UTF8);
            using (var mecab = new MeCabIpadic(new MeCabParam { DicDir = Path.Combine(dataDir, "mecab", "ipadic"), UseMemoryMappedFile = true }))
            using (var dict = JMDictLookup.Create(Path.Combine(dataDir, "dictionaries", "JMdict_e.gz"), Path.Combine(dataDir, "dictionaries", "JMdict_e.cache")))
            {
                var parser = new SentenceParser(mecab, dict);
                var glosser = new AutoGlosserNext(parser, dict, kana);
                var glosses = glosser.Gloss(input);
                var jsonWriter = new JsonTextWriter(Console.Out);
                jsonWriter.WriteStartArray();
                foreach (var gloss in glosses)
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("word");
                    jsonWriter.WriteValue(gloss.Foreign);
                    jsonWriter.WritePropertyName("definitions");
                    jsonWriter.WriteStartArray();
                    foreach (var glossCandidate in gloss.GlossCandidates)
                    {
                        jsonWriter.WriteValue(glossCandidate);
                    }
                    jsonWriter.WriteEndArray();
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
            }
        }
    }
}

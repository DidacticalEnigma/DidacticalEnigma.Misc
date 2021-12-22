using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JDict;
using JDict.Xml;

namespace DidacticalEnigma.Updater
{
    public class JMDictUpdaterProcess : JDictUpdaterProcess
    {
        private readonly IEnumerable<string> additionalCacheFilesToRemove;

        public JMDictUpdaterProcess(
            HttpClient httpClient,
            string url,
            string oldPath,
            string oldCachePath,
            string newPath,
            string newCachePath,
            string testedSchemaPath = null,
            IEnumerable<string> additionalCacheFilesToRemove = null) : base(
            "JMDict",
            httpClient,
            url,
            oldPath,
            oldCachePath,
            newPath,
            newCachePath,
            testedSchemaPath)
        {
            this.additionalCacheFilesToRemove = additionalCacheFilesToRemove 
                ?? Enumerable.Empty<string>();
        }

        protected override async Task Start()
        {
            await base.Start();
            foreach (var fileToRemove in additionalCacheFilesToRemove)
            {
                File.Delete(fileToRemove);
            }
        }

        protected override async Task CreateCache()
        {
            using (var jmdictLookup = await JMDictLookup.CreateAsync(newPath, newCachePath))
            {

            }
        }
    }
}
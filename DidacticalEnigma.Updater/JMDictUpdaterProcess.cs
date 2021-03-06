using System.Net.Http;
using System.Threading.Tasks;
using JDict;
using JDict.Xml;

namespace DidacticalEnigma.Updater
{
    public class JMDictUpdaterProcess : JDictUpdaterProcess
    {
        public JMDictUpdaterProcess(
            HttpClient httpClient,
            string url,
            string oldPath,
            string oldCachePath,
            string newPath,
            string newCachePath,
            string testedSchemaPath = null) : base(
            "JMDict",
            httpClient,
            url,
            oldPath,
            oldCachePath,
            newPath,
            newCachePath,
            testedSchemaPath)
        {
        }

        protected override async Task CreateCache()
        {
            using (var jmdictLookup = await JMDictLookup.CreateAsync(newPath, newCachePath))
            {

            }
        }
    }
}
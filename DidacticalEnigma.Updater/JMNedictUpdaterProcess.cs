using System.Net.Http;
using System.Threading.Tasks;
using JDict;

namespace DidacticalEnigma.Updater
{
    public class JMNedictUpdaterProcess : JDictUpdaterProcess
    {
        public JMNedictUpdaterProcess(
            HttpClient httpClient,
            string url,
            string oldPath,
            string oldCachePath,
            string newPath,
            string newCachePath,
            string testedSchemaPath = null) : base(
            "JMNedict",
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
            using (var jmdictLookup = await JMNedictLookup.CreateAsync(newPath, newCachePath))
            {

            }
        }
    }
}
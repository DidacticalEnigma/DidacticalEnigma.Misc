using System.Net.Http;
using System.Threading.Tasks;

namespace DidacticalEnigma.Updater
{
    public class KanjiDictUpdaterProcess : JDictUpdaterProcess
    {
        public KanjiDictUpdaterProcess(
            HttpClient httpClient,
            string url,
            string oldPath,
            string newPath) : base(
            "KanjiDict",
            httpClient,
            url,
            oldPath,
            null,
            newPath,
            null)
        {
        }

        protected override async Task CreateCache()
        {
            await Task.CompletedTask;
        }
    }
}
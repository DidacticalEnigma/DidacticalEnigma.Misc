using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DidacticalEnigma.Updater
{
    public static class HttpClientExtensions
    {
        public static async Task GetToFileAsync(this HttpClient httpClient, string url, string pathToFile)
        {
            using (var stream = await httpClient.GetStreamAsync(url)) 
            using (var outputStream = File.OpenWrite(pathToFile))
            {
                await stream.CopyToAsync(outputStream);
            }
        }
    }
}
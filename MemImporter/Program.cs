using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MagicTranslatorProjectMemImporter.MemApi;

namespace MemImporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var importerName = args[0];
            var address = args[1];
            var projectName = args[2];
            
            Console.WriteLine("Supply an access token (just the token, don't prefix it with \"Bearer\"), or press Enter to use without authentication:");
            var token = Console.ReadLine();
            var api = new DidacticalEnigmaMem(new Uri(address));
            if (!string.IsNullOrEmpty(token))
            {
                api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            switch (importerName)
            {
                case "magicTranslator":
                    await MagicTranslatorProjectImporter.Import(api, projectName, args.Skip(3).ToList());
                    break;
                case "immersionKit":
                    await ImmersionKitImporter.Import(api, projectName, args.Skip(3).ToList());
                    break;
            }
        }
    }
}
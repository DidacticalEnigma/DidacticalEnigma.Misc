using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DidacticalEnigma.Mem.Client;
using DidacticalEnigma.Mem.Client.MemApi;
using Optional.Unsafe;

namespace MemImporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var importerName = args[0];
            var address = args[1];
            var projectName = args[2];

            var uri = new Uri(address);
            var clientId = "MemImporter";
            
            var api = new DidacticalEnigmaMem(uri);
            var httpClient = api.HttpClient;

            var loginProcess = await httpClient.StartDeviceCodeLoginProcess(
                uri,
                clientId);

            if (!loginProcess.HasValue)
            {
                loginProcess.MatchNone(error => { Console.WriteLine(error); });
                return;
            }

            var loginProcessResult = loginProcess.ValueOrFailure();
            Console.WriteLine(
                $"The code is {loginProcessResult.UserCode}. Visit {loginProcessResult.VerificationUriComplete} to authenticate.");

            var resultOpt = await loginProcessResult.ProcessResultTask;
            
            if (!resultOpt.HasValue)
            {
                resultOpt.MatchNone(error => { Console.WriteLine(error); });
                return;
            }

            var result = resultOpt.ValueOrFailure();

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                api.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.AccessToken);
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
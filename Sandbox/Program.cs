// See https://aka.ms/new-console-template for more information

using DidacticalEnigma.SugoiSuite;

var client = new SugoiOfflineTranslatorClient(new HttpClient()
{
    BaseAddress = new Uri("http://localhost:14366")
});

Console.WriteLine(string.Join("\n", await client.GetTranslation(new []
{
    "こんにちは",
    "今からかりんち行ってもいいか?",
    "頼れるのはかりんしかいないんだ…"
})));
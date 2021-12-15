using DidacticalEnigma.Mem.Client;
using DidacticalEnigma.Mem.Client.MemApi;

var url = "URL GOES HERE";

var api = new DidacticalEnigmaMemViewModel();
api.Uri = url;
api.Initialize.Execute(null);

if (await api.TryLogin())
{
    Console.WriteLine("Login succeeded");
}
else
{
    Console.WriteLine("Login failed");
}

await api.Client.AddProjectAsync("MyTestProject");
await api.Client.DeleteProjectAsync("MyTestProject");

if (await api.TryLogout())
{
    Console.WriteLine("Logout succeeded");
}
else
{
    Console.WriteLine("Logout failed");
}

using System.IO;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting.Shell;

public class Program
{
    public static async Task Main()
    {
        var eng = global::IronPython.Hosting.Python.CreateEngine();
        var pythonCommandLine = new PythonCommandLine();
        pythonCommandLine.Run(eng, new Console(), new PythonConsoleOptions()
        {
            SkipImportSite = true
        });
    }

    public class Console : IConsole
    {
        public string ReadLine(int autoIndentSize)
        {
            Output.Write(new string(' ', autoIndentSize));
            return System.Console.ReadLine();
        }

        public void Write(string text, Style style)
        {
            System.Console.Write(text);
        }

        public void WriteLine(string text, Style style)
        {
            System.Console.WriteLine(text);
        }

        public void WriteLine()
        {
            System.Console.WriteLine();
        }

        public TextWriter Output { get; set; }
        public TextWriter ErrorOutput { get; set; }

        public Console()
        {
            ErrorOutput = System.Console.Error;
            Output = System.Console.Out;
        }
    }
}
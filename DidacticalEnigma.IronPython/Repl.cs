using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace DidacticalEnigma.IronPython
{
    public class Repl
    {
        private readonly TextWriter output;
        private readonly TextWriter errorOutput;
        private readonly ScriptEngine engine;
        private readonly ScriptScope scope;
        private readonly dynamic repr;

        public Repl(TextWriter output, TextWriter errorOutput)
        {
            engine = global::IronPython.Hosting.Python.CreateEngine();
            
            scope = engine.CreateScope();
            this.output = output;
            this.errorOutput = errorOutput;
        }

        public async Task IssueCommand(string code)
        {
            try
            {
                dynamic result = engine.Execute(code, scope);
                if (result != null)
                {
                    await this.output.WriteLineAsync(result.ToString());
                }
            }
            catch (Exception ex)
            {
                await this.errorOutput.WriteLineAsync(ex.Message);
            }
        }
    }
}
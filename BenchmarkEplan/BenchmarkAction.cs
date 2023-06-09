using System;
using System.Diagnostics;
using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;

namespace BenchmarkEplan
{
    public class BenchmarkAction : IEplAction
    {
        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = nameof(BenchmarkAction);
            Ordinal = 20;
            return true;
        }

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            string executionCount = string.Empty;
            oActionCallingContext.GetParameter("executionCount", ref executionCount);
            string commandLine = string.Empty;
            oActionCallingContext.GetParameter("commandLine", ref commandLine);

            var cli = new CommandLineInterpreter(false, false);
            int count = int.Parse(executionCount);
            var benchmark = new Benchmark(commandLine, count, s => cli.Execute(s));

            benchmark.Run();
            
            var filePath = new PathInfo().Documents + $@"\Benchmark_{DateTime.Now}.txt";
            benchmark.Report(filePath);

            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
            return true;
        }

        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }
}
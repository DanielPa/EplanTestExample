using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace BenchmarkEplan
{
    public class Benchmark
    {
        public  string CommandLine { get;  }
        public int ExecutionCount { get; }
        public List<TimeSpan> ExecutionTimes { get; } = new List<TimeSpan>();
        private Stopwatch _stopwatch = new Stopwatch();
        private readonly Action<string> _cliExecuteAction;

        public Benchmark(string commandLine, int executionCount, Action<string> cliExecute)
        {
            CommandLine = commandLine;
            ExecutionCount = executionCount;
            _cliExecuteAction = cliExecute;
        }   
        
        public void Run()
        {
            for (int i = 0; i < ExecutionCount; i++)
            {
                _stopwatch.Restart();
                _cliExecuteAction.Invoke(CommandLine);
                _stopwatch.Stop();
                ExecutionTimes.Add(_stopwatch.Elapsed);
            }
        }

        public void Report(string fileName)
        {
            var reportString = new StringBuilder();
            reportString.AppendLine($"Command line: {CommandLine}");
            reportString.AppendLine($"Execution count: {ExecutionCount}");
            reportString.AppendLine($"Total time: {GetTotalTime()}");
            reportString.AppendLine($"Average time: {GetAverageTime()}");
            reportString.AppendLine($"Min time: {GetMinTime()}");
            reportString.AppendLine($"Max time: {GetMaxTime()}");
            reportString.AppendLine($"Execution times: {string.Join(", ", ExecutionTimes)}");
            System.IO.File.WriteAllText(fileName, reportString.ToString());
            
        }

        private string GetAverageTime()
        {
            return ExecutionTimes.Average(span => span.Milliseconds).ToString("F3");
        }

        private string GetMinTime()
        {
            return ExecutionTimes.Min(span => span.Milliseconds).ToString("F3");
        }

        private string GetMaxTime()
        {
            return ExecutionTimes.Max(span => span.Milliseconds).ToString("F3");
        }

        private string GetTotalTime()
        {
            return ExecutionTimes.Sum(span => span.Milliseconds).ToString("F3");
        }
    }
}
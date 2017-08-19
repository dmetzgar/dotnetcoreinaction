using System.Reflection;
using Microsoft.Xunit.Performance.Api;

namespace CsvWriterPerfTests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var harness = new XunitPerformanceHarness(args))
            {
                var entryAssemblyPath = Assembly.GetEntryAssembly().Location;
                harness.RunBenchmarks(entryAssemblyPath);
            }
        }
    }
}

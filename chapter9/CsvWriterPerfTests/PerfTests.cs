using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xunit.Performance;
using CsvWriter;

namespace CsvWriterPerfTests
{
  public class PerfTests
  {
    static byte[] buffer = new byte[500000];

    [Benchmark(InnerIterationCount=10000)]
    public void BenchmarkSimpleWriter()
    {
      //var buffer = new byte[500000];
      var memoryStream = new MemoryStream(buffer);
      var values = new Dictionary<string, string>();
      values["Year"] = "2008";
      values["Title"] = "Iron Man";
      values["Production Studio"] = "Marvel Studios";

      foreach (var iteration in Benchmark.Iterations)
      {
        using (var streamWriter = new StreamWriter(memoryStream, Encoding.Default, 512, true))
        {
          using (iteration.StartMeasurement())
          {
            var simpleWriter = new SimpleWriter(streamWriter);
            simpleWriter.WriteHeader("Year", "Title", "Production Studio");
            for (int innerIteration = 0; innerIteration < Benchmark.InnerIterationCount; innerIteration++)
            {
              simpleWriter.WriteLine(values);
            }
            streamWriter.Flush();
          }
        }
        memoryStream.Seek(0, SeekOrigin.Begin);
      }
    }

    [Benchmark(InnerIterationCount=10000)]
    public void BenchmarkSimpleWriterToFile()
    {
      var values = new Dictionary<string, string>();
      values["Year"] = "2008";
      values["Title"] = "Iron Man";
      values["Production Studio"] = "Marvel Studios";
      int outerIterations = 0;
      foreach (var iteration in Benchmark.Iterations)
      {
        var fileStream = new FileStream($"tempfile{outerIterations++}.csv", FileMode.Create, FileAccess.Write);
        using (var streamWriter = new StreamWriter(fileStream, Encoding.Default, 512, false))
        {
          using (iteration.StartMeasurement())
          {
            var simpleWriter = new SimpleWriter(streamWriter);
            simpleWriter.WriteHeader("Year", "Title", "Production Studio");
            for (int innerIteration = 0; innerIteration < Benchmark.InnerIterationCount; innerIteration++)
            {
              simpleWriter.WriteLine(values);
            }
            streamWriter.Flush();
          }
        }
      }
    }

    [Benchmark(InnerIterationCount=10000)]
    public void BenchmarkSimpleWriterJoin()
    {
      //var buffer = new byte[500000];
      var memoryStream = new MemoryStream(buffer);

      foreach (var iteration in Benchmark.Iterations)
      {
        using (var streamWriter = new StreamWriter(memoryStream, Encoding.Default, 512, true))
        {
          using (iteration.StartMeasurement())
          {
            var simpleWriter = new SimpleWriter(streamWriter);
            simpleWriter.WriteHeader("Year", "Title", "Production Studio");
            for (int innerIteration = 0; innerIteration < Benchmark.InnerIterationCount; innerIteration++)
            {
              simpleWriter.WriteLine("2008", "Iron Man", "Marvel Studios");
            }
            streamWriter.Flush();
          }
        }
        memoryStream.Seek(0, SeekOrigin.Begin);
      }
    }

    [Benchmark(InnerIterationCount=10000)]
    public void BenchmarkSimpleWriterToFileJoin()
    {
      int outerIterations = 0;
      foreach (var iteration in Benchmark.Iterations)
      {
        var fileStream = new FileStream($"tempfile{outerIterations++}.csv", FileMode.Create, FileAccess.Write);
        using (var streamWriter = new StreamWriter(fileStream, Encoding.Default, 512, false))
        {
          using (iteration.StartMeasurement())
          {
            var simpleWriter = new SimpleWriter(streamWriter);
            simpleWriter.WriteHeader("Year", "Title", "Production Studio");
            for (int innerIteration = 0; innerIteration < Benchmark.InnerIterationCount; innerIteration++)
            {
              simpleWriter.WriteLine("2008", "Iron Man", "Marvel Studios");
            }
            streamWriter.Flush();
          }
        }
      }
    }
  }
}

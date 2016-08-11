using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApplication
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("Test1.csv");
      StreamReader sr = new StreamReader(stream);
      var csvReader = new CsvReader(sr);
      foreach (var line in csvReader.Lines)
        Console.WriteLine(line.First(p => p.Key == "Title").Value);
    }
  }
}

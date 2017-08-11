using System;
using System.IO;
using System.Linq;

namespace CsvParser
{
    class Program
    {
        static void Main(string[] args)
        {
             var csv = @"Year,Title,Production Studio
2008,Iron Man,Marvel Studios
2008,The Incredible Hulk,Marvel Studios
2008,Punisher: War Zone,Marvel Studios
2009,X-Men Origins: Wolverine,20th Century Fox
2010,Iron Man 2,Marvel Studios
2011,Thor,Marvel Studios
2011,X-Men: First Class,20th Century Fox
";

            var sr = new StringReader(csv);
            var csvReader = new CsvReader(sr);
            foreach (var line in csvReader.Lines)
                Console.WriteLine(line.First(p => p.Key == "Title").Value);
        }
    }
}

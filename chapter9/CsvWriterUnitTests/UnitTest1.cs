using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;
using CsvWriter;

namespace CsvWriterUnitTests
{
    public class UnitTest1
    {
        const string Marvel = "Marvel Studios";
        const string Fox = "20th Century Fox";

        [Fact]
        public void TestSimpleWriter()
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var simpleWriter = new SimpleWriter(streamWriter);
            WriteMarvelCsv(simpleWriter);
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(memoryStream);
            var testString = streamReader.ReadToEnd();
            
            var refString = GetReferenceMarvelCsv();
            Assert.Equal(refString, testString);
        }

        public static void WriteMarvelCsv(SimpleWriter simpleWriter)
        {
            simpleWriter.WriteHeader("Year", "Title", "Production Studio");
            var values = new Dictionary<string, string>();
            values["Year"] = "2008";
            values["Title"] = "Iron Man";
            values["Production Studio"] = Marvel;
            simpleWriter.WriteLine(values);
            values["Title"] = "The Incredible Hulk";
            simpleWriter.WriteLine(values);
            values["Title"] = "Punisher: War Zone";
            simpleWriter.WriteLine(values);
            values["Year"] = "2009";
            values["Title"] = "X-Men Origins: Wolverine";
            values["Production Studio"] = Fox;
            simpleWriter.WriteLine(values);
            values["Year"] = "2010";
            values["Title"] = "Iron Man 2";
            values["Production Studio"] = Marvel;
            simpleWriter.WriteLine(values);
            values["Year"] = "2011";
            values["Title"] = "Thor";
            simpleWriter.WriteLine(values);
            values["Title"] = "X-Men: First Class";
            values["Production Studio"] = Fox;
            simpleWriter.WriteLine(values);
        }

        private string GetReferenceMarvelCsv()
        {
            var stream = typeof(UnitTest1).GetTypeInfo().Assembly.
              GetManifestResourceStream(
                  "CsvWriterUnitTests.Marvel.csv");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace CsvWriter
{
    public class SimpleWriter
    {
        private TextWriter target;
        private string[] columns;

        public SimpleWriter(TextWriter target)
        {
            this.target = target;
        }

        public void WriteHeader(params string[] columns)
        {
            this.columns = columns;
            this.target.Write(columns[0]);
            for (int i = 1; i < columns.Length; i++)
                this.target.Write("," + columns[i]);
            this.target.WriteLine();
        }

        public void WriteLine(Dictionary<string, string> values)
        {
            this.target.Write(values[columns[0]]);
            for (int i = 1; i < columns.Length; i++)
                this.target.Write("," + values[columns[i]]);
            this.target.WriteLine();
            // this.target.WriteLine(string.Join(",", values.Values));
        }
    }
}

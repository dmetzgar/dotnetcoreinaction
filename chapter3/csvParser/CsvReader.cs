using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication
{
  public class CsvReader
  {
    private string[] columns;
    private TextReader source;
    
    public CsvReader(TextReader reader)
    {
      this.source = reader;
      var columnLine = reader.ReadLine();
      this.columns = columnLine.Split(',');
    }
    
    public IEnumerable<KeyValuePair<string, string>[]> Lines
    {
      get
      {
        string row;
        while ((row = this.source.ReadLine()) != null)
        {
          var cells = row.Split(',');
          var pairs = new KeyValuePair<string, string>[columns.Length];
          for (int col = 0; col < columns.Length; col++)
          {
            pairs[col] = new KeyValuePair<string, string>(columns[col], cells[col]);
          }
          
          yield return pairs;
        }
      }
    }
  }
}

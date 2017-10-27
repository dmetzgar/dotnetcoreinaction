using System;
using System.IO;
using System.Net.Http;

namespace MarkdownLiteTest
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var client = new HttpClient())
      {
        var response = client.GetAsync(
          "http://localhost:5000?container=somecontainer&blob=test.md")
          .Result;
        string markdown = response.Content.
          ReadAsStringAsync().Result;
        Console.WriteLine(markdown);
      }
    }
  }
}

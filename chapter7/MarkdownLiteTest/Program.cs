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
        /*
        var response = client.PostAsync(
          "http://localhost:5000",
          new StreamContent(
            new FileStream("test.md", FileMode.Open))
          ).Result;
        */
        
        var response = client.GetAsync(
          "http://localhost:5000?container=somecontainer&blob=test.md").Result;
        string markdown = response.Content.
          ReadAsStringAsync().Result;
        Console.WriteLine(markdown);
        
        response = client.PutAsync(
          "http://localhost:5000/somecontainer/foo.md",
          new StreamContent(
            new FileStream("test.md", FileMode.Open))
          ).Result;
        Console.WriteLine(response);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        
        response = client.DeleteAsync(
          "http://localhost:5000?container=somecontainer&blob=foo.md").Result;
        Console.WriteLine(response);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);      
      }
    }
  }
}

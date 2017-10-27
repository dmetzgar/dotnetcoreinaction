using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.MarkdownLite;
using Microsoft.Extensions.Configuration;

namespace MarkdownService
{
  [Route("/")]
  public class MdlController : Controller
  {
    private readonly IMarkdownEngine engine;
    private readonly string AccountName;
    private readonly string AccountKey;
    private readonly string BlobEndpoint;
    private readonly string ServiceVersion;
    
    public MdlController(IMarkdownEngine engine)
    {
      this.engine = engine;
      var configBuilder = new ConfigurationBuilder();
      configBuilder.AddJsonFile("config.json", true);
      var configRoot = configBuilder.Build();
      AccountName = configRoot["AccountName"];
      AccountKey = configRoot["AccountKey"];
      BlobEndpoint = configRoot["BlobEndpoint"];
      ServiceVersion = configRoot["ServiceVersion"];
    }
    
    [HttpPost]
    public async Task<IActionResult> Convert()
    {
      using (var reader = new StreamReader(Request.Body))
      {
        var markdown = await reader.ReadToEndAsync();
        var result = engine.Markup(markdown);
        return Content(result);
      }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBlob(
      string container, string blob)
    {
      var request = CreateRequest(HttpMethod.Get, container, blob);
      
      using (var client = new HttpClient())
      {
        var response = await client.SendAsync(request);
        var markdown = await response.Content.ReadAsStringAsync();
        var result = engine.Markup(markdown);
        return Content(result);
      }
    }
    
    private HttpRequestMessage CreateRequest(
      HttpMethod verb, string container, string blob)
    {
      var path = $"{container}/{blob}";
      var rfcDate = DateTime.UtcNow.ToString("R");
      var uri = new Uri(BlobEndpoint + path);
      var request = new HttpRequestMessage(HttpMethod.Get, uri);
      request.Headers.Add("x-ms-blob-type", "BlockBlob");
      request.Headers.Add("x-ms-date", rfcDate);
      request.Headers.Add("x-ms-version", ServiceVersion);
      
      var authHeader = GetAuthHeader(
        verb.ToString().ToUpper(), path, rfcDate);
      request.Headers.Add("Authorization", authHeader);

      return request;      
    }

    private string GetAuthHeader(string verb, string path, string rfcDate)
    {
      var devStorage = BlobEndpoint.StartsWith("http://127.0.0.1:10000") ?
                       $"/{AccountName}" : "";
      var signme = $"{verb}\n\n\n\n\n\n\n\n\n\n\n\n" +
                    "x-ms-blob-type:BlockBlob\n" +
                   $"x-ms-date:{rfcDate}\n" +
                   $"x-ms-version:{ServiceVersion}\n" +
                   $"/{AccountName}{devStorage}/{path}";

      string signature = "";
      using (var sha = new HMACSHA256(
        System.Convert.FromBase64String(AccountKey)))
      {
        var data = Encoding.UTF8.GetBytes(signme);
        signature = System.Convert.ToBase64String(sha.ComputeHash(data));
      }

      return $"SharedKey {AccountName}:{signature}";
    }
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
    private static readonly HttpClient client = new HttpClient();
    private readonly IMarkdownEngine engine;
    private string AccountName;
    private string AccountKey;
    private string BlobEndpoint;
    private string ServiceVersion;
 
    public MdlController(IMarkdownEngine engine, IConfigurationRoot configRoot)
    {
      this.engine = engine;
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
    public async Task<IActionResult> GetBlob(string container, string blob)
    {
      var request = CreateRequest(HttpMethod.Get, container, blob);
      var contentType = blob == null ? "text/xml" : "text/html";
      
      var response = await client.SendAsync(request);
      var responseContent = await response.Content.ReadAsStringAsync();
      if (blob != null)
        responseContent = engine.Markup(responseContent);
      return Content(responseContent, contentType);
    }
    
    [HttpPut("{container}/{blob}")]
    public async Task<IActionResult> PutBlob(string container, string blob)
    {
      var contentLen = this.Request.ContentLength;
      var request = CreateRequest(HttpMethod.Put, container, blob, contentLen);
      request.Content = new StreamContent(this.Request.Body);
      request.Content.Headers.Add("Content-Length", contentLen.ToString());
      
      var response = await client.SendAsync(request);
      if (response.StatusCode == HttpStatusCode.Created)
        return Created($"{AccountName}/{container}/{blob}", null);
      else
        return Content(await response.Content.ReadAsStringAsync());
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBlob(string container, string blob)
    {
      var request = CreateRequest(HttpMethod.Delete, container, blob);
      
      var response = await client.SendAsync(request);
      if (response.StatusCode == HttpStatusCode.Accepted)
        return NoContent();
      else
        return Content(await response.Content.ReadAsStringAsync());
    }
    
    private HttpRequestMessage CreateRequest(HttpMethod verb, string container, string blob = null, long? contentLen = default(long?))
    {
      string path;
      Uri uri;
      if (blob != null)
      {
        path = $"{container}/{blob}";
        uri = new Uri(BlobEndpoint + path);
      }
      else if (container != null)
      {
        path = container;
        uri = new Uri($"{BlobEndpoint}{path}?restype=container&comp=list");
      }
      else 
      {
        path = "";
        uri = new Uri($"{BlobEndpoint}?comp=list");
      }

      var rfcDate = DateTime.UtcNow.ToString("R");
      var request = new HttpRequestMessage(verb, uri);
      if (blob != null)
        request.Headers.Add("x-ms-blob-type", "BlockBlob");
      request.Headers.Add("x-ms-date", rfcDate);
      request.Headers.Add("x-ms-version", ServiceVersion);
      
      var authHeader = GetAuthHeader(verb.ToString().ToUpper(), path, rfcDate, contentLen, blob == null, container == null);
      request.Headers.Add("Authorization", authHeader);
      
      return request;
    }
    
    private string GetAuthHeader(string verb, string path, string rfcDate, long? contentLen, bool listBlob, bool listContainer)
    {
      var devStorage = BlobEndpoint.StartsWith("http://127.0.0.1:10000") ?
                       $"/{AccountName}" : "";
      var signme = $"{verb}\n\n\n" +
                   $"{contentLen}\n" + 
                    "\n\n\n\n\n\n\n\n" +
                   (listBlob ? "" : "x-ms-blob-type:BlockBlob\n") +
                   $"x-ms-date:{rfcDate}\n" + 
                   $"x-ms-version:{ServiceVersion}\n" +
                   $"/{AccountName}{devStorage}/{path}";
      if (listContainer)
        signme +=   "\ncomp:list";
      else if (listBlob)
        signme +=   "\ncomp:list\nrestype:container";

      string signature;             
      using (var sha = new HMACSHA256(System.Convert.FromBase64String(AccountKey)))
      {
        var data = Encoding.UTF8.GetBytes(signme);
        signature = System.Convert.ToBase64String(sha.ComputeHash(data));
      }
      
      return $"SharedKey {AccountName}:{signature}";
    }
  }
}

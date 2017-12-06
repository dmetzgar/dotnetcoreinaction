using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.DocAsCode.MarkdownLite;

namespace MarkdownService
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      
      var builder = new GfmEngineBuilder(new Options());
      var engine = builder.CreateEngine(new HtmlRenderer());
      services.AddSingleton<IMarkdownEngine>(engine);

      var configBuilder = new ConfigurationBuilder();
      configBuilder.AddJsonFile("config.json", true);
      var configRoot = configBuilder.Build();
      services.AddSingleton<IConfigurationRoot>(configRoot);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}

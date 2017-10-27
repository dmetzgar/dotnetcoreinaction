using Microsoft.AspNetCore.Builder;
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
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}

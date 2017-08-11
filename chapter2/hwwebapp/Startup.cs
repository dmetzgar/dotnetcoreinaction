using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace hwwebapp
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.Run(async (context) =>
                await context.Response.WriteAsync("Hello World!")
            );
        }
    }
}

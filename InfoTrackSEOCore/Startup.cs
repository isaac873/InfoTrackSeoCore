using System.IO;
using System.Net.Http;
using InfoTrackSEOCore.Configuration;
using InfoTrackSEOCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrackSEOCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var conf = Configuration.Get<InfoTrackSeoCoreConfiguration>();

            services
              .AddScoped<ISearchScraper, SearchScraper>()
              .AddScoped<IScrapeParser, ScrapeParser>()
              .AddScoped<IWebRequester, WebRequester>()
			  .AddSingleton<ILogger, TextLogger>()
              .AddSingleton(conf)
			  .AddHttpClient()
              .AddCors()
              .AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseCors(x =>
            {
              x.AllowAnyMethod();
              x.AllowAnyOrigin();
            });
            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}

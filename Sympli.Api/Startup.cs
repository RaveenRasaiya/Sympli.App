using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sympli.Api.Extensions;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using Sympli.Search.Providers;
using Sympli.Search.Services;

namespace Sympli.Api
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
            services.AdCorsDefault();
            services.AddJsonDefault();
            services.AddRouting();
            services.Configure<SearchSettings>(Configuration.GetSection("SearchSettings"));
            services.AddMemoryCache();
            services.AddScoped<IHttpApiClient, HttpApiClient>();
            services.AddScoped<ISearchRequstValidator, SearchRequstValidator>();
            services.AddScoped<IBingBotService, BingBotService>();
            services.AddScoped<IGoogleBotService, GoogleBotService>();
            services.AddScoped<IBotProvider, BotProvider>();
            services.AddScoped<IParallelBotService, ParallelBotService>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();         
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "InfoTrack API");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

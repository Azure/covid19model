using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Web.Data;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAzureClients((azureClientFactoryBuilder) =>
            {
                if (Environment.IsDevelopment())
                {
                    azureClientFactoryBuilder.UseCredential(new InteractiveBrowserCredential());
                }
                else
                {
                    azureClientFactoryBuilder.UseCredential(new ManagedIdentityCredential());
                }
                azureClientFactoryBuilder.AddSecretClient(new Uri("https://adfiksencovid19.vault.azure.net/"));
                azureClientFactoryBuilder.AddBlobServiceClient(new Uri("https://adfiksencovid19.blob.core.windows.net/"));
            });
            services.AddTransient<IModelDataProvider, ModelDataProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

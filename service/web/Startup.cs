using Azure.Identity;
using Azure.Storage;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
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
                    // use login locally
                    // See https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows
                    // in terminal, run dotnet user-secrets set "Storage:Blob:StorageAccessKey" "{storageKeyHere}"
                    // where storageKeyHere is found in the Access keys tab of our storage account.
                    // Ensure running with ASPNETCORE_ENVIRONMENT flag set to "Development"
                    azureClientFactoryBuilder.AddBlobServiceClient(new Uri("https://msfticlblockblob.blob.core.windows.net/"), new StorageSharedKeyCredential("msfticlblockblob", Configuration["Storage:Blob:StorageAccessKey"]));
                }
                else
                {
                    // use MSI on website
                    azureClientFactoryBuilder.UseCredential(new ManagedIdentityCredential());
                    azureClientFactoryBuilder.AddBlobServiceClient(new Uri("https://msfticlblockblob.blob.core.windows.net/"));
                }
            });
            services.AddTransient<IModelDataProvider, ModelDataProvider>();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddSignIn("AzureAd", Configuration, options => Configuration.Bind("AzureAd", options));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather_Function.Services;

[assembly: FunctionsStartup(typeof(Weather_Function.Startup))]
namespace Weather_Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                return new TableServiceClient(config.GetValue<string>("AzureWebJobsStorage"));
            });
            builder.Services.AddSingleton(sp => new TableService(sp.GetService<TableServiceClient>()));


            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                return new BlobServiceClient(config.GetValue<string>("AzureWebJobsStorage"));
            });
            builder.Services.AddSingleton(sp => new BlobService(sp.GetService<BlobServiceClient>()));
        }
    }
}

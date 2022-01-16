using Azure.Storage.Blobs;
using BabaFunkeInvoiceManager.Repository;
using BabaFunkeInvoiceManager.Services;
using BabaFunkeInvoiceManager.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;

namespace BabaFunkeInvoiceManager
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddSingleton<IInvoiceService, InvoiceService>();
            builder.Services.AddSingleton<IBlobManager, BlobManager>();
            builder.Services.AddSingleton<IEmailService, EmailService>();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton(x =>
            {
                CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(config.GetValue<string>("Values:AzureWebJobsStorage"));
                CloudTableClient cloudTableClient = cloudStorage.CreateCloudTableClient();
                CloudTable InvoiceTable = cloudTableClient.GetTableReference("InvoiceManager");
                return InvoiceTable;
            });

            builder.Services.AddSingleton(x =>
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(config.GetValue<string>("Values:AzureWebJobsStorage"));
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("invoices");
                return blobContainerClient;
            });

            builder.Services
                .AddFluentEmail(config.GetValue<string>("EmailSettings:SenderEmail"), "Mama Funke")
                .AddSendGridSender(config.GetValue<string>("EmailSettings:SendGridKey"));
        }
    }
}
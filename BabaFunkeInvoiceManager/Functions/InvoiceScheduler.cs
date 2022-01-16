using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(BabaFunkeInvoiceManager.Startup))]
namespace BabaFunkeInvoiceManager.Functions
{
    public class InvoiceScheduler
    {
        private readonly IBlobManager _blobManager;
        private readonly IEmailService _emailService;
        private readonly IInvoiceService _invoiceService;

        public InvoiceScheduler(IBlobManager blobManager,
            IEmailService emailService,
            IInvoiceService invoiceService)
        {
            _blobManager = blobManager;
            _emailService = emailService;
            _invoiceService = invoiceService;
        }

        [FunctionName("InvoiceScheduler")]
        //Runs Monthly on the first Monday of each month at 9 A.M.
        public async Task Run([TimerTrigger("0 0 9 1-7 * Mon")] TimerInfo myTimer, ILogger log)
        {
            BlobDetail blobDetail = null;
            try
            {
                //Get the current month as a string
                var intCurrentMonth = DateTime.Now.Month;
                var currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(intCurrentMonth);

                //Retrieve the blob (file) from the Storage Container
                blobDetail = await _blobManager.GetBlob(currentMonth);

                if (blobDetail == null)
                {
                    return;
                }

                //Send the email 
                var emailSent = await _emailService.SendEmail(blobDetail);

                if(!emailSent)
                {
                    return;
                }

                //Add record as reference to the table storage
                var invoice = new Invoice
                {
                    ReferenceId = blobDetail.BlobName,
                    Category = "Invoice",
                    BlobUri = blobDetail.BlobUri,
                    Status = "Sent"
                };

                var result = await _invoiceService.AddInvoice(invoice);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (blobDetail != null)
                {
                    blobDetail.BlobStream.Dispose();
                }
            }
        }
    }
}
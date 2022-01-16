using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Services.Interfaces;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<bool> SendEmail(BlobDetail blobDetail)
        {
            var attachment = new Attachment
            {
                Data = blobDetail.BlobStream,
                ContentType = "application/pdf",
                Filename = blobDetail.BlobName
            };

            var recipients = new List<Address>()
                {
                    new Address {EmailAddress = "mamafunke.customer1@example.com"},
                    new Address {EmailAddress = "mamafunke.customer2@example.com"}
                };

            var result = await _fluentEmail
                .To(recipients)
                .Subject($"{blobDetail.CurrentMonth} Invoice for Genii Games App")
                .Body($"Hello Customer,\n\nFind attached {blobDetail.CurrentMonth}'s invoice.\n\nBest regards\nMama Funke")
                .BCC("mamafunke@daddycreates.com")
                .Attach(attachment)
                .SendAsync();

            return result.Successful;
        }
    }
}
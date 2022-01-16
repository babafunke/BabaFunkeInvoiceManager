using BabaFunkeInvoiceManager.Entities;
using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Repository;
using BabaFunkeInvoiceManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
        }

        public async Task<bool> AddInvoice(Invoice invoice)
        {
            if(invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice));
            }

            try
            {
                var invoiceTableEntity = new InvoiceTableEntity
                {
                    RowKey = invoice.ReferenceId,
                    PartitionKey = invoice.Category,
                    Status = invoice.Status,
                    Sent = invoice.Sent,
                    IsArchived = invoice.IsArchived,
                    BlobUri = invoice.BlobUri
                };

                await _invoiceRepository.AddInvoiceTableEntity(invoiceTableEntity);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
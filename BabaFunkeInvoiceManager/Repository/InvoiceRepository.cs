using BabaFunkeInvoiceManager.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly CloudTable _invoiceTable;

        public InvoiceRepository(CloudTable invoiceTable)
        {
            _invoiceTable = invoiceTable;
        }

        public async Task<bool> AddInvoiceTableEntity(InvoiceTableEntity invoiceTableEntity)
        {
            var operation = TableOperation.Insert(invoiceTableEntity);

            await _invoiceTable.ExecuteAsync(operation);

            return true;
        }
    }
}
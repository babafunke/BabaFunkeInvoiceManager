using BabaFunkeInvoiceManager.Entities;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Repository
{
    public interface IInvoiceRepository
    {
        Task<bool> AddInvoiceTableEntity(InvoiceTableEntity invoiceTableEntity);
    }
}
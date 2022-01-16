using BabaFunkeInvoiceManager.Models;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<bool> AddInvoice(Invoice invoice);
    }
}
using BabaFunkeInvoiceManager.Models;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services.Interfaces
{
    public interface IBlobManager
    {
        Task<BlobDetail> GetBlob(string currentMonth);
    }
}
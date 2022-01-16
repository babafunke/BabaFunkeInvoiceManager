using BabaFunkeInvoiceManager.Models;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(BlobDetail blobDetail);
    }
}
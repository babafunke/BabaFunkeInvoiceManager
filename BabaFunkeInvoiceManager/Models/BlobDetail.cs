using System.IO;

namespace BabaFunkeInvoiceManager.Models
{
    /// <summary>
    /// This handles the response of a query to our Storage container where the pre-designed invoices have been uploaded.
    /// The pdf file is returned as a Stream, its location as the BlobUri, file name as BlobName and the month as CurrentMonth.
    /// </summary>
    public class BlobDetail
    {
        public BlobDetail(Stream blobStream, string blobName, string blobUri, string currentMonth)
        {
            BlobStream = blobStream;
            BlobName = blobName;
            BlobUri = blobUri;
            CurrentMonth = currentMonth;
        }

        public Stream BlobStream { get; set; }
        public string BlobName { get; set; }
        public string BlobUri { get; set; }
        public string CurrentMonth { get; set; }
    }
}
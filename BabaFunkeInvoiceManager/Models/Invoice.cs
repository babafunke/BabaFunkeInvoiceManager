using System;

namespace BabaFunkeInvoiceManager.Models
{
    public class Invoice
    {
        public string ReferenceId { get; set; }
        public string Category { get; set; } = "Invoice";
        public string Status { get; set; }
        public DateTime Sent { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; } = false;
        public string BlobUri { get; set; }
    }
}
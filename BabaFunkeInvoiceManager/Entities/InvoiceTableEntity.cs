using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace BabaFunkeInvoiceManager.Entities
{
    /// <summary>
    /// The object that directly interacts with our Azure Table Storage.
    /// Its properties map to those of the Invoice Model except for the ReferenceId and Catgory.
    /// The properties Reference Id and Category map to the inherited RowKey and PartitionKey which
    /// come with TableEntity by default.
    /// </summary>
    public class InvoiceTableEntity : TableEntity
    {
        public string Status { get; set; }
        public DateTime Sent { get; set; }
        public bool IsArchived { get; set; }
        public string BlobUri { get; set; }
    }
}
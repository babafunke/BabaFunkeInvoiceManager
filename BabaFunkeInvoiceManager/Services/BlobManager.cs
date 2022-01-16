using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BabaFunkeInvoiceManager.Services
{
    public class BlobManager : IBlobManager
    {
        private readonly BlobContainerClient _container;

        public BlobManager(BlobContainerClient container)
        {
            _container = container;
        }

        public async Task<BlobDetail> GetBlob(string currentMonth)
        {
            var blobItems = await GetAllBlobs();

            var blobItem = blobItems.SingleOrDefault(b => b.Name.Contains(currentMonth));

            if (blobItem == null)
            {
                return null;
            }

            var blobClient = _container.GetBlobClient(blobItem.Name);

            BlobDownloadInfo response = await blobClient.DownloadAsync();

            var blobStream = new MemoryStream();

            await response.Content.CopyToAsync(blobStream);

            blobStream.Seek(0, SeekOrigin.Begin);

            return new BlobDetail(blobStream, blobItem.Name, blobClient.Uri.ToString(), currentMonth);
        }

        private async Task<List<BlobItem>> GetAllBlobs()
        {
            try
            {
                var blobs = new List<BlobItem>();

                var resultSegment = _container.GetBlobsAsync().AsPages(default, null);

                await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    blobs.AddRange(blobPage.Values.ToList());
                }

                return blobs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
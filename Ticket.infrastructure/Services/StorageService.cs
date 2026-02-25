using Azure.Storage.Blobs;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Services;

namespace TicketAzure.infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;

        public StorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("ticket-images");
        }

        public async Task<string> GetSignedUrlAsync(string fileName, TimeSpan expirationTime, CancellationToken cancellationToken = default)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            if(await blobClient.ExistsAsync(cancellationToken))
            {
                var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationTime));

                return sasUri.AbsoluteUri;
            }

            return string.Empty;

        }

        public async Task<string> UploadFileAsync(string containerName, string fileName, string base64, CancellationToken cancellationToken = default)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            var bytes = Convert.FromBase64String(base64);

            using (var stream = new MemoryStream(bytes))
            {
                blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
            }

            return fileName;
        }
    }
}

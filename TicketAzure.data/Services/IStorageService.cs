using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.Core.Services
{
    public interface IStorageService 
    {
        Task<string> UploadFileAsync(string containerName, string fileName, string base64, CancellationToken cancellationToken = default);

        Task<string> GetSignedUrlAsync(string fileName, TimeSpan expirationTime ,CancellationToken cancellationToken = default);
    }
}

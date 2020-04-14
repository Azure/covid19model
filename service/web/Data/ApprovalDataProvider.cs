using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace Web.Data
{
    /// <summary>
    /// Provides model inferred data
    /// </summary>
    public class ApprovalDataProvider : IApprovalDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobStorageClient"></param>
        /// TODO: fix - this is not blob storage
        public ApprovalDataProvider(BlobServiceClient blobStorageClient)
        {

        }

        public Task<bool> IsDataApproved(DateTime dateRequested)
        {
            // TODO: verify if a given date's data has been approved
            return Task.FromResult(false);
        }
    }
}

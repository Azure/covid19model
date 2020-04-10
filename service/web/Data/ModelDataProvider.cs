using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Web.Data
{
    public class ModelDataProvider : IModelDataProvider
    {
        private readonly BlobContainerClient _blobContainerClient;

        public ModelDataProvider(BlobServiceClient blobServiceClient)
        { 
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("covidmodels");
        }

        public async Task<Stream> GetPredictionDataAsync(string country)
        {
            var blobName = GetBlobName("plot", country);
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var blobResponse = await blobClient.DownloadAsync();
            var requestStatus = blobResponse.GetRawResponse().Status;
            if (requestStatus == (int)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (requestStatus >= (int)HttpStatusCode.BadRequest)
            {
                throw new InvalidOperationException("Unexpected response from blob store when pulling model data");
            }

            return blobResponse.Value.Content;
        }

        public async Task<Stream> GetInterventionDataAsync(string country)
        {
            var blobName = GetBlobName("intervention", country);
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var blobResponse = await blobClient.DownloadAsync();
            var requestStatus = blobResponse.GetRawResponse().Status;
            if (requestStatus == (int)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (requestStatus >= (int)HttpStatusCode.BadRequest)
            {
                throw new InvalidOperationException("Unexpected response from blob store when pulling model data");
            }

            return blobResponse.Value.Content;
        }

        /// <summary>
        /// Given a data type to lookup, and a country to look it up for, returns the name of the blob representing
        /// that data.
        /// </summary>
        private string GetBlobName(string dataType, string country)
        {
            string countrySuffix;
            if (country == null)
            {
                countrySuffix = string.Empty;
            }
            else
            {
                countrySuffix = $"-{country}";
            }


            return $"base-{dataType}{countrySuffix}.csv";
        }
    }
}

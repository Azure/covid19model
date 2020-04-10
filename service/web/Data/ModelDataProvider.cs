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
            var blobClient = _blobContainerClient.GetBlobClient($"base-plot-{country}.csv");
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
            var blobClient = _blobContainerClient.GetBlobClient($"base-intervention-{country}.csv");
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
    }
}

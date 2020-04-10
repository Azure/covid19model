using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Web.Data
{
    public class ModelDataProvider : IModelDataProvider
    {
        private readonly BlobServiceClient _client;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly Response<BlobServiceProperties> _props;

        public ModelDataProvider(BlobServiceClient blobServiceClient)
        {
            _client = blobServiceClient;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("covidmodels");
        }

        public async Task<Stream> GetModelDataAsync()
        {
            var blobClient = _blobContainerClient.GetBlobClient(GetBlobName());
            ////using (var stream = new MemoryStream())
            ////{
                var blobResponse = await blobClient.DownloadAsync();
                if (blobResponse.GetRawResponse().Status >= (int)HttpStatusCode.BadRequest)
                {
                    throw new InvalidOperationException("Unexpected response from blob store when pulling model data");
                }

                return blobResponse.Value.Content;
                ////var formatter = new BinaryFormatter();
                ////stream.Seek(0, SeekOrigin.Begin);
                ////return formatter.Deserialize(stream);
            ////}
        }

        // TODO: update this daily to the correctly ingested model data
        private string GetBlobName() => "predictions.csv";
    }
}

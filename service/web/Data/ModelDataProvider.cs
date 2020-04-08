using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Web.Data
{
    public class ModelDataProvider : IModelDataProvider
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ModelDataProvider(BlobServiceClient azureClientFactory)
        {
            _blobServiceClient = azureClientFactory;
        }

        public async Task<object> GetModelDataAsync()
        {
            return new
            {
                foo="bar"
            };
            ////var modelData = await _blobClient.DownloadAsync();

            ////var formatter = new BinaryFormatter();
            ////modelData.Value.Content.Seek(0, SeekOrigin.Begin);
            ////return formatter.Deserialize(modelData.Value.Content);
        }
    }
}

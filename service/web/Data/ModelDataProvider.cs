using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Web.Data
{
    public class ModelDataProvider : IModelDataProvider
    {
        private readonly BlobContainerClient _blobContainerClient;

        public ModelDataProvider(BlobServiceClient blobServiceClient)
        {
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("covid19model");
        }

        public async Task<object> GetModelDataAsync()
        {
            return null;
            ////var modelData = await 

            ////var formatter = new BinaryFormatter();
            ////modelData.Value.Content.Seek(0, SeekOrigin.Begin);
            ////return formatter.Deserialize(modelData.Value.Content);
        }
    }
}

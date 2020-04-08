using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Web.Data
{
    public class ModelDataProvider : IModelDataProvider
    {
        private readonly BlobClient _blobClient;

        public ModelDataProvider(BlobClient blobClient)
        {
            _blobClient = blobClient;
        }

        public async Task<object> GetModelDataAsync()
        {
            var modelData = await _blobClient.DownloadAsync();

            var formatter = new BinaryFormatter();
            modelData.Value.Content.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(modelData.Value.Content);
        }
    }
}

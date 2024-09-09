using Azure.Storage.Blobs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Weather_Function
{
    public class BlobService
    {
        private static readonly string weatherFolder = "weather-forcasts";
        private readonly BlobServiceClient _serviceClient;

        public BlobService(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task UploadFileAsync(Stream file, string fileName)
        {
            var containerClient = _serviceClient.GetBlobContainerClient(weatherFolder);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(file);
        }

        public async Task<object> DownloadBlobAsJsonAsync(string name)
        {
            var containerClient = _serviceClient.GetBlobContainerClient(weatherFolder);
            if (!await containerClient.ExistsAsync()) return null;

            var blob = containerClient.GetBlobClient(name);
            if (!await blob.ExistsAsync()) return null;

            using var stream = new MemoryStream();
            await blob.DownloadToAsync(stream);
            stream.Position = 0;
            var serializer = new JsonSerializer();

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            return serializer.Deserialize(jsonTextReader);
        }
    }
}

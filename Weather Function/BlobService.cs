using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Weather_Function.Models;

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

        public async Task UploadFileAsync(Stream file, string fileName, WeatherLog wl)
        {
            var containerClient = _serviceClient.GetBlobContainerClient(weatherFolder);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName.ToLower());
            if (!await blobClient.ExistsAsync())
            {
                await blobClient.UploadAsync(file);
                wl.Success = true;
                wl.Message = "Weather record saved sucessfully";
            }
            else
            {
                wl.Success = false;
                wl.Message = "Weather record already stored";
            }
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

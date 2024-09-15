using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Weather_Function.Functions
{
    public class GetWeather
    {
        private readonly ILogger _logger;
        private readonly BlobService _blobService;

        public GetWeather(ILoggerFactory loggerFactory, BlobService blobService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _blobService = blobService;
        }

        /// <summary>
        /// Gets weather file
        /// </summary>
        [FunctionName(nameof(GetWeather))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = $"{nameof(GetWeather)}/{{city}}/{{blob}}")] HttpRequest req, string city, long blob)
        {
            object result;
            try
            {
                var requestString = $"{city}/{blob.ToString()}".ToLower();
                result = await _blobService.DownloadBlobAsJsonAsync(requestString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve blob: {blob}");
                return new InternalServerErrorResult();
            }

            if (result != null)
            {
                _logger.LogInformation($"Sent weather data from blob: {blob}");
                return new JsonResult(result);
            }

            var message = $"Weather data file {blob} not found";
            _logger.LogInformation(message);
            return new NotFoundObjectResult(message);
        }
    }
}

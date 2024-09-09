using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather_Function.Functions
{
    public class WeatherFetcher
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly Uri _weather;
        private readonly BlobService _blobService;
        private readonly TableService _tableService;

        public WeatherFetcher(ILoggerFactory loggerFactory, IConfiguration config, BlobService blobService, TableService tableService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _httpClient = new HttpClient();
            _blobService = blobService;
            _tableService = tableService;
            _weather = new Uri(config.GetValue<string>("WeatherApiUrl"));
        }

        /// <summary>
        /// Periodically fetches Weather data for London
        /// </summary>
        [FunctionName(nameof(WeatherFetcher))]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            var resp = await _httpClient.GetAsync(_weather);

            var success = resp.IsSuccessStatusCode;
            var timestamp = DateTime.UtcNow.Ticks;

            await _tableService.UploadWeatherLogAsync(timestamp, success);

            if (success)
                await _blobService.UploadFileAsync(await resp.Content.ReadAsStreamAsync(), timestamp.ToString());
        }
    }
}

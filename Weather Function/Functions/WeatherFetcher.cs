using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Weather_Function.Models;

namespace Weather_Function.Functions
{
    public class WeatherFetcher
    {
        private readonly ILogger _logger;
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly HttpClient _httpClient;
        private readonly Uri _weather;
        private readonly string _city;

        public WeatherFetcher(ILoggerFactory loggerFactory, IConfiguration config, BlobService blobService, TableService tableService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _blobService = blobService;
            _tableService = tableService;

            _httpClient = new HttpClient();
            _city = config.GetValue<string>("WeatherCity");
            var weatherApi = config.GetValue<string>("WeatherApiUrl").Replace("{{City}}", _city);
            _weather = new Uri(weatherApi);
        }

        /// <summary>
        /// Periodically fetches Weather data for London
        /// </summary>
        [FunctionName(nameof(WeatherFetcher))]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            var resp = await _httpClient.GetAsync(_weather);

            var success = resp.IsSuccessStatusCode;

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var wl = new WeatherLog
            {
                PartitionKey = _city,
                RowKey = timestamp.ToString(),
                TimeOfEntry = timestamp,
            };

            if (success)
            {
                var stream = await resp.Content.ReadAsStreamAsync();

                var serializer = new JsonSerializer();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                var forcast = serializer.Deserialize<WeatherForcast>(jsonTextReader);
                stream.Position = 0;

                timestamp = forcast.dt;
                wl.WeatherTimestamp = timestamp;
                var filename = $"{_city}/{timestamp}";

                try
                {
                    await _blobService.UploadFileAsync(stream, filename, wl);
                }
                catch (Exception ex)
                {
                    wl.Success = false;
                    wl.Message = "Failed to save weather record";
                    _logger.LogError(ex, wl.Message);
                }
            }
            else
            {
                wl.Success = false;
                wl.Message = "Failed to retreive weather record";
                _logger.LogError(wl.Message);
            }

            try
            {
                await _tableService.UploadWeatherLogAsync(timestamp, wl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store weather log in database");
            }
        }
    }
}

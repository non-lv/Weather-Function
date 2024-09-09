using Azure.Data.Tables;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weather_Function.Models;

namespace Weather_Function
{
    public class TableService
    {
        private static readonly string TableName = "WeatherLogs";
        private readonly TableServiceClient _tableServiceClient;

        public TableService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task UploadWeatherLogAsync(long timestamp, bool success)
        {
            await _tableServiceClient.CreateTableIfNotExistsAsync(TableName);
            var tableClient = _tableServiceClient.GetTableClient(TableName);

            var weatherLog = new WeatherLog
            {
                WeatherTimestamp = timestamp,
                RowKey = timestamp.ToString(),
                Success = success,
                PartitionKey = "London"
            };

            tableClient.AddEntity(weatherLog);
        }

        public string GetWeatherLogsAsJson(DateTime from, DateTime to)
        {
            var tableClient = _tableServiceClient.GetTableClient(TableName);

            var results = tableClient.Query<WeatherLog>(e => e.WeatherTimestamp >= from.Ticks && e.WeatherTimestamp <= to.Ticks).ToList();

            return JsonConvert.SerializeObject(results);
        }
    }
}

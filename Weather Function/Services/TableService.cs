using Azure.Data.Tables;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weather_Function.Models;

namespace Weather_Function.Services
{
    public class TableService
    {
        private static readonly string TableName = "WeatherLogs";
        private readonly TableServiceClient _tableServiceClient;

        public TableService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task UploadWeatherLogAsync(long timestamp, WeatherLog wl)
        {
            await _tableServiceClient.CreateTableIfNotExistsAsync(TableName);
            var tableClient = _tableServiceClient.GetTableClient(TableName);

            tableClient.AddEntity(wl);
        }

        public string GetWeatherLogsAsJson(DateTimeOffset from, DateTimeOffset to)
        {
            var tableClient = _tableServiceClient.GetTableClient(TableName);
            var results = tableClient.Query<WeatherLog>(e => e.TimeOfEntry >= from.ToUnixTimeSeconds() && e.TimeOfEntry <= to.ToUnixTimeSeconds()).ToList();

            return JsonConvert.SerializeObject(results);
        }
    }
}

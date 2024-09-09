using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Weather_Function.Functions
{
    public class GetWeatherLogs
    {
        private readonly ILogger _logger;
        private readonly TableService _tableService;

        public GetWeatherLogs(ILoggerFactory loggerFactory, TableService tableService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _tableService = tableService;
        }

        /// <summary>
        /// Gets logs from a certain time period
        /// </summary>
        [FunctionName(nameof(GetWeatherLogs))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            DateTime from = DateTime.ParseExact(req.Query["from"], "yyMMdd-HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(req.Query["to"], "yyMMdd-HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            var results = _tableService.GetWeatherLogsAsJson(from, to);
            return new OkObjectResult(JsonConvert.SerializeObject(results));
        }
    }
}

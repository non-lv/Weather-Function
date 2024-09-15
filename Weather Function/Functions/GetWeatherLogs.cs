using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Weather_Function.Models;

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
            var from = DateTimeOffset.ParseExact(req.Query["from"], "yyMMdd-HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            var to = DateTimeOffset.ParseExact(req.Query["to"], "yyMMdd-HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            string results;
            try
            {
                results = _tableService.GetWeatherLogsAsJson(from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve weather logs");
                return new InternalServerErrorResult();
            }
            var wl = JsonConvert.DeserializeObject<List<WeatherLog>>(results);
            return new JsonResult(wl.Select(x => x.ToWeatherLogResponse()));
        }
    }
}

using Azure;
using Azure.Data.Tables;
using System;

namespace Weather_Function.Models;

public class WeatherLog : ITableEntity
{
    public long TimeOfEntry { get; set; }
    public long WeatherTimestamp { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    // References the City of WeatherLog
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

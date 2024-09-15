using System;
using Weather_Function.Models;

namespace Weather_Function.Extensions
{
    public static class WeatherLogExtensions
    {
        public static WeatherLogResponse ToWeatherLogResponse(this WeatherLog wl) => new()
        {
            Success = wl.Success,
            TimeOfEntry = TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeSeconds(wl.TimeOfEntry).UtcDateTime, TimeZoneInfo.Local),
            City = wl.PartitionKey,
            FileName = wl.WeatherTimestamp,
            Message = wl.Message
        };
    }
}

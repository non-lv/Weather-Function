using System;

namespace Weather_Function.Models
{
    public class WeatherLogResponse
    {
        public bool Success { get; set; }
        public DateTime TimeOfEntry { get; set; }
        public string City { get; set; }
        public long FileName { get; set; }
        public string Message { get; set; }
    }
}

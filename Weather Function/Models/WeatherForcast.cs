﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Weather_Function.Models
{
    // Openweather API response structure
    public class WeatherForcast
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
#nullable enable
        public Rain? rain { get; set; }
        public Snow? snow { get; set; }
#nullable disable
        public long dt { get; set; }
        public Sys sys { get; set; }
        public long timezone { get; set; }
        public uint id { get; set; }
        public string name { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public uint id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }

    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Rain
    {
        [JsonProperty("1h")]
        public int one_h { get; set; }
        [JsonProperty("3h")]
        public int three_h { get; set; }
    }

    public class Snow
    {
        [JsonProperty("1h")]
        public int one_h { get; set; }
        [JsonProperty("3h")]
        public int three_h { get; set; }
    }

    public class Sys
    {
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }
}

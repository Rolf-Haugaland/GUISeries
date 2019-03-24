using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GUISeries
{
    public class CLSerie
    {
        public string status;
        [JsonProperty("canonicalTitle")]
        public string name;
        public int? episodeCount;
        public List<string> genres;
        [JsonProperty("startDate")]
        public DateTime aired;
    }
}

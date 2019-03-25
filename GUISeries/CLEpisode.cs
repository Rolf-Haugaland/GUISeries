using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GUISeries
{
    public class CLEpisode
    {
        public int length;
        [JsonProperty("canonicalTitle")]
        public string name;
        public string synopsis;
        [JsonProperty("airdate")]
        public DateTime airDate;
        [JsonProperty("number")]
        public int EpisodeNumber;
        public string showName;
    }
}

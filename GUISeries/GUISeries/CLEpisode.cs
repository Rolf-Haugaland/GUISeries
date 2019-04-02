using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GUISeries
{
    public class CLEpisode
    {//If there isnt a JsonProperty mentioned it is either implemented manually or it has the same variable name as the JsonProperty, hence not needed.
        public int? length;
        [JsonProperty("canonicalTitle")]
        public string episodeName;
        public string synopsis;
        [JsonProperty("airdate")]
        public DateTime? airDate;
        [JsonProperty("number")]
        public int? EpisodeNumber;
        public string showName;
        public string seasonNumber;
    }
}

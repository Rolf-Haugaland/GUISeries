using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GUISeries
{
    public class CLSerie : CLEpisode
    {
        public string status;
        [JsonProperty("canonicalTitle")]
        public string name;
        public int? episodeCount;
        public List<string> genres;
        [JsonProperty("startDate")]
        public DateTime? aired;
        public string linkToEpisodes;
        public int? totalLength;
        public bool? NSFW;
        public string ageRating;
        public string linkToGenres;
        //This is used for reference to which ID it has in the database
        public int DBID;
    }
}

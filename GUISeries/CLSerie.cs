using System;
using System.Collections.Generic;

namespace GUISeries
{
    class CLSerie : CLEpisode
    {
        public List<CLEpisode> episodes;
        public string status;
        public string name;
        public int? episodeCount;
        public List<string> genres;
        public DateTime aired;
    }
}

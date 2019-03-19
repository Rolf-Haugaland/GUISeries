using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUISeries
{
    class CLSerie : CLEpisode
    {
        List<CLEpisode> episodes;
        string status;
        string name;
        int? episodeCount;
        List<string> genres;
        DateTime aired;
    }
}

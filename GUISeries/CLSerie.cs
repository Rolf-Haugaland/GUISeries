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
        public string linkToEpisodes;


        void RunThisShit()
        {
            int x = 0;
            int y = 41;
            int AmountBetweenNumbersInt = AmountBetweenNumbers(x, y);

            int AmountOfTimesToRunInt = AmountOfTimes(AmountBetweenNumbersInt);
            
            //I got the amount of times to run it, now i just need the fucking rest.

            for(int i = 0; AmountOfTimesToRunInt > i; i++)
            {
                run(x, ShortenInt(x-y));
            }
        }

        int ShortenInt(int num1)
        {
            if (num1 > 20)
                return 20;
            else
                return num1;
        }

        void run(int startingNum, int amount)
        {

        }

        int AmountBetweenNumbers(int one, int two)
        {
            if (one != 0)
                return (two - one) + 1;
            else
                return two - one;
        }

        int AmountOfTimes(int runs)
        {
            if(runs < 20)
            {
                return 1;
            }
            else if(runs > 20)
            {
                for(int i = 0; runs > i; i++)
                {
                    runs -= 20;
                    if (runs <= 0)
                        return i;
                }
            }

            return -1;
        }
    }
}

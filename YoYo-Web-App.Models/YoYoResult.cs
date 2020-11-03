using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoYo_Web_App.Models
{
    public class YoYoResult
    {
        public List<AthleteFitnessResults> athleteFitnessResults { get; set; }
        public List<FitnessRating> fitnessRatings { get; set; }
        public List<string> possibleResults { get; set; }
    }
}

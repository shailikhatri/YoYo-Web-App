using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoYo_Web_App.Models
{
    public class FitnessRating
    {
        public int LevelNo { get; set; }
        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get;set;}
        public int ShuttleNo { get; set; }
        public decimal Speed { get; set; }
        public decimal LevelTime { get; set; }
        public string CommulativeTime { get; set; }
        public string StartTime { get; set; }
        public decimal ApproxVo2Max { get; set; }
    }
}

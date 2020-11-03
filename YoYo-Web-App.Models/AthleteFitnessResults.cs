using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YoYo_Web_App.Models
{
    public class AthleteFitnessResults
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWarned { get; set; }
        public bool IsStopped { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public string result { get; set; }
    }
}

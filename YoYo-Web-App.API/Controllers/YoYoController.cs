using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.Controllers
{
    [ApiController]
    [Route("api/yoyo")]
    public class YoYoController : Controller
    {
        private readonly ILogger<YoYoController> _logger;

        public YoYoController(ILogger<YoYoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            YoYoResult yoYoResult = new YoYoResult();
            // load json file data into session

            //read json file and convert into list

            string fileData = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "JSON", "fitnessrating_beeptest.json"));
            var fitnessRatingsList = JsonConvert.DeserializeObject<List<FitnessRating>>(fileData).OrderBy(c => c.CommulativeTime);

            //add same level no 
            int i = 0;
            foreach (var item in fitnessRatingsList)
            {
                if (item.ShuttleNo == 1)
                    i++;
                item.LevelNo = i;
            }

            List<string> possibleResults = fitnessRatingsList.Select(c => new { Result = c.SpeedLevel + " - " + c.ShuttleNo }).Select(c => c.Result).ToList();

            //load Athlete List
            string athletefileData = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "JSON", "athlete.json"));
            var athleteRatingsList = JsonConvert.DeserializeObject<List<AthleteFitnessResults>>(athletefileData).OrderBy(c => c.Id).ToList();

            yoYoResult.athleteFitnessResults = athleteRatingsList;
            yoYoResult.fitnessRatings = fitnessRatingsList.ToList();
            yoYoResult.possibleResults = possibleResults;

            return Ok(yoYoResult);
        }

    }
}
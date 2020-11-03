using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private const string BaseURL = "https://localhost:44309/api/yoyo/";
        public IActionResult Index()
        {
            YoYoResult yoYoResult = new YoYoResult();
            using (var client = new HttpClient())
            {
                string URL = BaseURL + "Index";
                //HTTP GET

                client.BaseAddress = new Uri(URL);
                var httpResponse = client.GetAsync(URL).GetAwaiter();
                if (httpResponse.GetResult().IsSuccessStatusCode)
                {
                    yoYoResult = JsonConvert.DeserializeObject<YoYoResult>(httpResponse.GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }

            //clear all session keys
            HttpContext.Session.Clear();

            //add Athlete data into session
            HttpContext.Session.SetString("AthleteRatings", JsonConvert.SerializeObject(yoYoResult.athleteFitnessResults));
            HttpContext.Session.SetString("FitnessRatings", JsonConvert.SerializeObject(yoYoResult.fitnessRatings));

            return View(yoYoResult);
        }

        [Route("GetNextShuttle")]
        [HttpPost]
        public IActionResult GetNextShuttle(int levelNo, int shuttleNo)
        {
            string fitnessRatingsData = HttpContext.Session.GetString("FitnessRatings");
            var fitnessRatingsList = JsonConvert.DeserializeObject<List<FitnessRating>>(fitnessRatingsData);

            var item = fitnessRatingsList.Where(c => c.LevelNo == levelNo && c.ShuttleNo == shuttleNo + 1).FirstOrDefault();
            if (item == null)
                item = fitnessRatingsList.Where(c => c.LevelNo == levelNo + 1).FirstOrDefault();

            return Ok(item);
        }

        [Route("WarnAthlete")]
        [HttpPost]
        public ActionResult WarnAthlete(int Id)
        {

            string athletefileData = HttpContext.Session.GetString("AthleteRatings");
            var athleteRatingsList = JsonConvert.DeserializeObject<List<AthleteFitnessResults>>(athletefileData);

            var athlete = athleteRatingsList.Where(c => c.Id == Id).FirstOrDefault();
            if (athlete != null)
                athlete.IsWarned = true;

            HttpContext.Session.SetString("AthleteRatings", JsonConvert.SerializeObject(athleteRatingsList));
            return Json(JsonConvert.SerializeObject(athleteRatingsList));
        }

        [Route("StopAthlete")]
        [HttpPost]
        public ActionResult StopAthlete(int Id, int speedLevel, int shuttleNo)
        {

            string athletefileData = HttpContext.Session.GetString("AthleteRatings");
            var athleteRatingsList = JsonConvert.DeserializeObject<List<AthleteFitnessResults>>(athletefileData);

            var athlete = athleteRatingsList.Where(c => c.Id == Id && c.IsStopped==false).FirstOrDefault();
            if (athlete != null)
            {
                athlete.IsStopped = true;
                athlete.ShuttleNo = shuttleNo;
                athlete.SpeedLevel = speedLevel;
                athlete.result = athlete.SpeedLevel + " - " + athlete.ShuttleNo;
            }

            HttpContext.Session.SetString("AthleteRatings", JsonConvert.SerializeObject(athleteRatingsList));
            return Ok(athleteRatingsList.ToArray());
        }

        [Route("ChangeAthleteResult")]
        [HttpPost]

        public ActionResult ChangeAthleteResult(int Id, string result)
        {

            string athletefileData = HttpContext.Session.GetString("AthleteRatings");
            var athleteRatingsList = JsonConvert.DeserializeObject<List<AthleteFitnessResults>>(athletefileData);

            var athlete = athleteRatingsList.Where(c => c.Id == Id).FirstOrDefault();
            if (athlete != null)
            {
                athlete.result = result;
            }

            HttpContext.Session.SetString("AthleteRatings", JsonConvert.SerializeObject(athleteRatingsList));
            return Ok();
        }
    }
}

using BKIZ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BKIZ.Controllers
{
    public class ProfitabilityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Result(Profitability model)
        {
            if (ModelState.IsValid)
            {
                float amountOfMintedHNTdaily=0;
                int numberOfHotSpots=0;

                string path = "https://api.helium.io/v1/stats";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var json = await client.GetStringAsync(path);
                    StatsRoot data = JsonConvert.DeserializeObject<StatsRoot>(json);
                    numberOfHotSpots = data.data.counts.hotspots_online;
                }


                path = "https://api.helium.io/v1/rewards/sum?min_time=-1%20day&bucket=day";
                response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var json = await client.GetStringAsync(path);
                    RootRewards dataReward = JsonConvert.DeserializeObject<RootRewards>(json);
                    amountOfMintedHNTdaily = dataReward.data[0].total;
                }

                var monthlyAvgRewardsPerHotspot = amountOfMintedHNTdaily / numberOfHotSpots * 30;
                var numOfMonths = model.CostOfHotspot / (monthlyAvgRewardsPerHotspot * model.CurrentHNTMarketPrice);
                ViewBag.Message = Math.Round(numOfMonths).ToString();
            }

            return View();
        }
    }
}

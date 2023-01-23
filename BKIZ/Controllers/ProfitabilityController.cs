using BKIZ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using CoinMarketCap;
using Microsoft.OData.Edm;

namespace BKIZ.Controllers
{
    public class ProfitabilityController : Controller
    {
        public async Task<ActionResult> Index()
        {

            string path = "https://api.helium.io/v1/stats";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            HttpResponseMessage response = await client.GetAsync(path);

            int numberOfHotSpots = 0;
            if (response.IsSuccessStatusCode)
            {
                var json = await client.GetStringAsync(path);
                StatsRoot data = JsonConvert.DeserializeObject<StatsRoot>(json);
                numberOfHotSpots = data.data.counts.hotspots_online;
            }
          
            var paths = GeneratePaths();
            List<float> avgRewardsByMonth = new List<float>();
            foreach (var rewardPath in paths)
            {
                response = await client.GetAsync(rewardPath);

                if (response.IsSuccessStatusCode)
                {
                    var json = await client.GetStringAsync(rewardPath);
                    RootRewardsMonthly reward = JsonConvert.DeserializeObject<RootRewardsMonthly>(json);
                    var amountOfMintedHNTmonthly = reward.data.total;
                    avgRewardsByMonth.Add((float)amountOfMintedHNTmonthly / (float)numberOfHotSpots);
                }

            }
            ViewBag.ChartData = avgRewardsByMonth;

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Result(Profitability model)
        {
            if (ModelState.IsValid)
            {
                float amountOfMintedHNTdaily = 0;
                int numberOfHotSpots = 0;

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

        public List<string> GeneratePaths()
        {
            List<string> months = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                string newValue;
                if (i < 10)
                {
                    newValue = "0" + i.ToString();
                }
                else
                {
                    newValue = i.ToString();
                }

                months.Add(newValue);
            }

            List<string> paths = new List<string>();
            for (int i = 0; i < months.Count - 1; i++)
            {
                paths.Add(GeneratePath(months[i], months[i + 1]));
            }
            paths.Add("https://api.helium.io/v1/rewards/sum?min_time=2021-12-01T00:00:00Z&max_time=2021-12-31T00:00:00Z");
            return paths;
        }

        public string GeneratePath(string minMonth, string maxMonth)
        {
            return "https://api.helium.io/v1/rewards/sum?min_time=2021-" + minMonth + "-01T00:00:00Z&max_time=2021-" + maxMonth + "-01T00:00:00Z";
        }
    }
}

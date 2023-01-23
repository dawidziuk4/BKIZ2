using BKIZ.Models.Block;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BKIZ.Controllers
{
    public class BlockController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string blockHeightPath = "https://api.helium.io/v1/blocks/height";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            HttpResponseMessage response = await client.GetAsync(blockHeightPath);

            if (response.IsSuccessStatusCode)
            {
                var json = await client.GetStringAsync(blockHeightPath);
                BlockRoot data = JsonConvert.DeserializeObject<BlockRoot>(json);
                ViewBag.Message = data;
                return View();
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> Result(BlockData model)
        {
            if (ModelState.IsValid)
            {
                string path = "https://api.helium.io/v1/blocks/" + model.height.ToString();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var json = await client.GetStringAsync(path);
                    BlockInfoRoot data = JsonConvert.DeserializeObject<BlockInfoRoot>(json);

                    DateTime dat_Time = new System.DateTime(1965, 1, 1, 0, 0, 0, 0);
                    dat_Time = dat_Time.AddSeconds(data.data.time);
                    data.data.datetime = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();
                    
                    ViewBag.Message = data;
                }

            }

            return View();
        }
    }
}

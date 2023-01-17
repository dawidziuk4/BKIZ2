using BKIZ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BKIZ.Controllers
{
    public class HotspotController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Result(HotspotName hotspotName)
        {
            var splittedName = hotspotName.name.ToLower().Split(" ");
            var pathName = splittedName[0] + "-" + splittedName[1] + "-" + splittedName[2];
            string path = "https://api.helium.io/v1/hotspots/name/" + pathName;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            HttpResponseMessage response = await client.GetAsync(path);

            double lat;
            double lng;

            if (response.IsSuccessStatusCode)
            {
                var json = await client.GetStringAsync(path);
                HotspotRoot data = JsonConvert.DeserializeObject<HotspotRoot>(json);
                ViewBag.Message = data;

                lat = data.data[0].lat;
                lng = data.data[0].lng;
            }

            return View();

        }
    }
}

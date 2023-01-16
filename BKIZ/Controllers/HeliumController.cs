using BKIZ.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BKIZ.Controllers
{
    public class HeliumController : Controller
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

            if (response.IsSuccessStatusCode)
            {
                var json = await client.GetStringAsync(path);
                Root data = JsonConvert.DeserializeObject<Root>(json);
                ViewBag.Message = data;
                return View();
            }
            return null;
        }
    }

}

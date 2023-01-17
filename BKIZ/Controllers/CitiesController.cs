using Microsoft.AspNetCore.Mvc;

namespace BKIZ.Controllers
{
    public class CitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

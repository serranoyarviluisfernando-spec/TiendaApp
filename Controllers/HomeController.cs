using Microsoft.AspNetCore.Mvc;

namespace TallerMecanico.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
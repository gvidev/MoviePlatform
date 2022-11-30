using Microsoft.AspNetCore.Mvc;

namespace MoviePlatform.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

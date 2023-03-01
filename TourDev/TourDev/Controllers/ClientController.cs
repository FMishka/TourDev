using Microsoft.AspNetCore.Mvc;

namespace TourDev.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

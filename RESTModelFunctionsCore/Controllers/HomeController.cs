using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.Inventory
{
    public class AsnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Receiving()
        {
            return View();
        }

    }
}

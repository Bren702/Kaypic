using Microsoft.AspNetCore.Mvc;

namespace KayPic.Controllers
{
    public class OrganisationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

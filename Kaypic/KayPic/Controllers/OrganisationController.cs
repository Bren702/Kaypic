using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KayPic.Controllers
{
    [Authorize]
    public class OrganisationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

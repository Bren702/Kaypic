using KayPic.Models;
using Microsoft.AspNetCore.Mvc;

namespace KayPic.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            var contacts = new List<MessagingPersona>
            {
                new MessagingPersona
                {
                    mp_id = 1,
                    mp_fname = "Foot",
                    mp_lname = "Rosemont",
                    mp_email = "footrosemont@example.com",
                    mp_status = Status.active,
                    mp_category = PersonaCategory.parent,
                    created_at = DateTime.Now.AddDays(-1)
                },
                new MessagingPersona
                {
                    mp_id = 2,
                    mp_fname = "Kadi",
                    mp_lname = "Ndacke",
                    mp_email = "kadi@example.com",
                    mp_status = Status.active,
                    mp_category = PersonaCategory.player,
                    created_at = DateTime.Now.AddDays(-2)
                }
            };
            ViewBag.MessagingPersona = contacts;

            return View();

        }
    }
}

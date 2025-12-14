using KayPic.Data;
using KayPic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Controllers
{
    public class CommunicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            /*
            var contacts = new List<MessagingPersona>
            {
                new MessagingPersona
                {
                    mp_id = 1,
                    mp_fname = "William",
                    mp_lname = "Leduc",
                    mp_email = "wleduc@example.com",
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
            */
            return View();

        }

        public IActionResult Messages()
        {
            return View();
        }

        public IActionResult Calendrier()
        {
            return View();
        }

        public async Task<IActionResult> Annonces()
        {
            var news = await _context.News
                .Include(n => n.author_mp)
                .Where(n => n.news_status == Status.active)
                .OrderByDescending(n => n.date_posted)
                .ToListAsync();
            
            return View(news);
        }
    }
}

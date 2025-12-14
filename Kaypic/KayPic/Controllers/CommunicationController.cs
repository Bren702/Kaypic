using KayPic.Data;
using KayPic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Controllers
{
    [Authorize]
    public class CommunicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Persona> _userManager;

        public CommunicationController(ApplicationDbContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IActionResult> Messages()
        {
            // Get the current authenticated user
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser == null)
            {
                return Challenge();
            }

            // Find the MessagingPersona(s) linked to the current user by email
            var messagingPersonas = await _context.MessagingPersonas
                .Where(mp => mp.mp_email == currentUser.Email)
                .Select(mp => mp.mp_id)
                .ToListAsync();

            // If the user has no MessagingPersona, return empty list
            if (!messagingPersonas.Any())
            {
                ViewBag.Chats = new List<MessagingChat>();
                return View();
            }

            // Filter chats where the user is a member
            var userChats = await _context.MessagingChatPersonas
                .Where(mcp => messagingPersonas.Contains(mcp.mcp_mp_id) && mcp.mcp_status == Status.active)
                .Include(mcp => mcp.mcp_mc)
                    .ThenInclude(mc => mc.created_by_mp)
                .Include(mcp => mcp.mcp_mc)
                    .ThenInclude(mc => mc.mc_team_season)
                .Select(mcp => mcp.mcp_mc)
                .Distinct()
                .ToListAsync();

            ViewBag.Chats = userChats;
            return View();
        }

        public IActionResult Calendrier()
        {
            return View();
        }

        public async Task<IActionResult> Annonces()
        {
            // Get the current authenticated user
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser == null)
            {
                return Challenge();
            }

            // Find all TeamSeasons the user belongs to via MessagingPersona
            var userTeamSeasonIds = await _context.MessagingPersonas
                .Where(mp => mp.mp_email == currentUser.Email && mp.mp_status == Status.active)
                .Select(mp => mp.mp_team_season_id)
                .Distinct()
                .ToListAsync();

            // If the user doesn't belong to any team seasons, return empty list
            if (!userTeamSeasonIds.Any())
            {
                return View(new List<News>());
            }

            // Filter news to only those published in the user's team seasons
            var news = await _context.News
                .Include(n => n.author_mp)
                .Include(n => n.news_ts)
                .Where(n => n.news_status == Status.active && userTeamSeasonIds.Contains(n.news_ts_id))
                .OrderByDescending(n => n.date_posted)
                .ToListAsync();
            
            return View(news);
        }
    }
}

using KayPic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Data
{
    public class ApplicationDbContext : IdentityDbContext<Persona>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Parent> Parents { get; set; } = default!;
        public DbSet<TeamManager> TeamManagers { get; set; } = default!;
        public DbSet<TeamSeason> TeamSeasons { get; set; } = default!;
    }
}
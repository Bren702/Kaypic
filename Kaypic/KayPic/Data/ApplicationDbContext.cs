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
        public DbSet<MessagingPersona> MessagingPersonas { get; set; } = default!;
        public DbSet<MessagingChat> MessagingChats { get; set; } = default!;
        public DbSet<MessagingChatPersona> MessagingChatPersonas { get; set; } = default!;
        public DbSet<MessagingChatPersonaMessage> MessagingChatPersonaMessages { get; set; } = default!;
        public DbSet<MessagingMedia> MessagingMedias { get; set; } = default!;
        public DbSet<News> News { get; set; } = default!;
    }
}
using KayPic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Data
{
    public class IdentityDb : IdentityDbContext<Persona>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options)
            : base(options) { }
    }
}



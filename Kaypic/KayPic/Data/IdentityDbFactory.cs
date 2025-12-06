namespace KayPic.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    namespace KayPic.Data
    {
        public class IdentityDbFactory : IDesignTimeDbContextFactory<IdentityDb>
        {
            public IdentityDb CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<IdentityDb>();
                optionsBuilder.UseSqlite("Data Source=Identity.db");

                return new IdentityDb(optionsBuilder.Options);
            }
        }
    }
}

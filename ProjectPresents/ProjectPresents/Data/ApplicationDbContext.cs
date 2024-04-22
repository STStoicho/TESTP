using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectPresents.Data
{
    public class ApplicationDbContext : IdentityDbContext<Client>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Present> Presents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Aplied> Aplieds { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}

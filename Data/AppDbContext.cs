using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Models;

namespace NehuenOrganico.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }

}

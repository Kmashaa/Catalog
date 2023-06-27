using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Catalog.Models;


namespace Catalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Credit> Credits { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Catalog.Entities.Logg> Loggs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
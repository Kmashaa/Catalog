using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Catalog.Models;


namespace Catalog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Credit> Credits { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
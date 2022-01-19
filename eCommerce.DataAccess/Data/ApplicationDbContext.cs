using eCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}

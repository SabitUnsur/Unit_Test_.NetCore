using Microsoft.EntityFrameworkCore;

namespace RealWordUnitTest.Web.Models
{
    public class Context: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=UnitTestDB;Username=postgres;Password=123");
        }

        public DbSet<Product> Products { get; set; }
    }
}

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
        public DbSet<Category> Categories { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category { Id=1 , Name = "Kalemler"} , new Category { Id = 2,Name = "Defterler"});
        }*/
    }
}

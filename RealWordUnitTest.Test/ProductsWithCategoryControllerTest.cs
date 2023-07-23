using Microsoft.EntityFrameworkCore;
using RealWordUnitTest.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWordUnitTest.Test
{
    public class ProductsWithCategoryControllerTest
    {
        protected DbContextOptions<Context> _contextOptions;

        public ProductsWithCategoryControllerTest(DbContextOptions<Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public void Seed()
        {
            using (Context context = new Context())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Categories.Add(new Category() { Name = "Kırtasiye" });
                context.Categories.Add(new Category() { Name = "Malzeme" });
                context.SaveChanges();
                context.Products.Add(new Product() { CategoryId = 3, Name = "Kalem10", Price = 100, Stock = 100, Color = "Red" });
                context.Products.Add(new Product() { CategoryId = 4, Name = "Kalem20", Price = 100, Stock = 100, Color = "Blue" });
                context.SaveChanges();
            }
        }
    }
}

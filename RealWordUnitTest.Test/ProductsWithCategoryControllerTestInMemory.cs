using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealWordUnitTest.Web.Controllers;
using RealWordUnitTest.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWordUnitTest.Test
{
    public class ProductsWithCategoryControllerTestInMemory : ProductsWithCategoryControllerTest
    {
        /*public ProductsWithCategoryControllerTestInMemory()
        {
            SetContextOptions(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("UnitTestInMemoryDB").Options);
        }

        public async Task Create_ModelValidProduct_ReturnsRedirectToActionWithProduct()
        {
            using (var context = new Context())
            {
                var category = context.Categories.First();
                var newProduct = new Product { Name = "Kalem30", Price = 200, Stock = 100 };
                newProduct.CategoryId = category.Id;
                var controller = new ProductsWithCategoryController(context);
                var result = controller.Create(newProduct);
                Assert.IsType<RedirectToActionResult>(result);

            }

            // Veritabanına eklendi mi kontrolü. EfCore track ettiği için emin olmak adına bu şekilde yapıldı.
            using (var context = new Context())
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);
                Assert.Equal(newProduct.Name, product.Name);
            }
        }*/
    }
}

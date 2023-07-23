using Microsoft.AspNetCore.Mvc;
using Moq;
using RealWordUnitTest.Web.Controllers;
using RealWordUnitTest.Web.Helpers;
using RealWordUnitTest.Web.Models;
using RealWordUnitTest.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWordUnitTest.Test
{
    public class ProductApiControllerTest
    {
        //Console üzerinden test için "dotnet test" yazılır. 
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsApiController _apiController;
        private readonly Helper _helper;
        private List<Product> products;

        public ProductApiControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _apiController = new ProductsApiController(_mockRepo.Object);
            _helper = new Helper();
            products = new List<Product>() {new Product { ID=1,Name="Kalem",Price=100,Stock=-1000,Color="Red"},
            new Product { ID=2,Name="Silgi",Price=100,Stock=200,Color="Blue"}};
        }

        [Theory]
        [InlineData(4,5,9)]
        public void Add_SampleValues_ReturnTotal(int a,int b,int total)
        {
            var result = _helper.add(a,b);
            Assert.Equal(total, result);
        }

        [Fact]
        public async void GetProducts_ActionExecutes_ReturnOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _apiController.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(2, returnProduct.ToList().Count);
        }

        [Theory]
        [InlineData(0)]
        public async void GetProduct_IdInvalid_ReturnNotFound(int productID)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var result = await _apiController.GetProduct(productID);
            Assert.IsType<NotFoundResult>(result); //Apide NotFound ile içerisinde bir obje de dönüyor olsaydı NotFoundObjectResult kullanırdık
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetProduct_IdValid_ReturnOkResult(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var result = await _apiController.GetProduct(productID); 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productID, returnProduct.ID);
        }

        [Theory]
        [InlineData(1)]
        public async void PutProduct_IdIsNotEqualProduct_ReturnBadRequestResult(int productID)
        {
            var product = products.First(x => x.ID == productID);
            var result = await _apiController.PutProduct(2, product);
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void PutProduct_ActionsExecutes_ReturnNoContentResult(int productID)
        {
            var product = products.First(x=>x.ID == productID);
            _mockRepo.Setup(x => x.Update(product));
            var result = await _apiController.PutProduct(productID,product);
            _mockRepo.Verify(x => x.Update(product), Times.Once);
            Assert.IsType<NoContentResult>(result); //Interface  dönen metotlar için bu şekilde yazılır.
        }

        [Fact]
        public async void PostProduct_ActionExecutes_ReturnCreatedAction()
        {
            var product = products.First();
            _mockRepo.Setup(x => x.Create(product)).Returns(Task.CompletedTask);
            var result = await _apiController.PostProduct(product);
            var createdActionResult = Assert.IsType<CreatedAtActionResult>(result);
             _mockRepo.Verify(x => x.Create(product),Times.Once);
            Assert.Equal("GetProduct", createdActionResult.ActionName);
        }

        [Theory]
        [InlineData(0)]
        public async void DeleteProduct_IdInvalid_ReturnNotFound(int productID)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var resultNotFound = await _apiController.DeleteProduct(productID);
            Assert.IsType<NotFoundResult>(resultNotFound.Result); //API içinde ActionResult olarak döndüğü için Result olarak alındı, Sınıf dönerse bu şekilde yapılır.
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_ActionExecutes_ReturnNoContent(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product); // Burada GetById'yi de mockladık çünkü yapılmadığında bir önceki testten dolayı null dönüyor ve test fail oluyordu.
            _mockRepo.Setup(x => x.Delete(product)); //Delete metotu geriye bir şey dönemeyeceğinden dolayı sadece çalışmasını kontrol ettik.
            var NoContentresult = await _apiController.DeleteProduct(productID);
            _mockRepo.Verify(x => x.Delete(product),Times.Once);
            Assert.IsType<NoContentResult>(NoContentresult.Result);
        }
    }
}

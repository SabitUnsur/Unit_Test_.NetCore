using Microsoft.AspNetCore.Mvc;
using Moq;
using RealWordUnitTest.Web.Controllers;
using RealWordUnitTest.Web.Models;
using RealWordUnitTest.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RealWordUnitTest.Test
{
    public class ProductControllerTest
    {
        //MockBehavior.Strict() ---> Bütün her şeyi mocklamak gerekir.

        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductController _productController;
        private List<Product> products;

        public ProductControllerTest()
        {
            _mockRepo=new Mock<IRepository<Product>>();
            _productController = new ProductController(_mockRepo.Object);

            products = new List<Product>() {new Product { ID=1,Name="Kalem",Price=100,Stock=-1000,Color="Red"},
            new Product { ID=2,Name="Silgi",Price=100,Stock=200,Color="Blue"}};
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _productController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
           _mockRepo.Setup(repo=>repo.GetAll()).ReturnsAsync(products);
            var result = await _productController.Index();
            var ViewResult= Assert.IsType<ViewResult>(result);
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(ViewResult.Model);
            Assert.Equal<int>(2, productList.Count());
        }

        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _productController.Details(null);
            var redirect= Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void Details_IdInvalid_ReturnNotFound()
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetByID(0)).ReturnsAsync(product);
            var result = await _productController.Details(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }


        [Theory]
        [InlineData(1)]
        public async void Detail_ValidId_ReturnProduct(int productID)
        {
            Product product = products.First(x => x.ID == productID);
            _mockRepo.Setup(repo => repo.GetByID(productID)).ReturnsAsync(product);
            var result=await _productController.Details(productID);
            var ViewResult = Assert.IsType<ViewResult>(result);
            var resultProduct=Assert.IsAssignableFrom<Product>(ViewResult.Model);
            Assert.Equal(product.ID, resultProduct.ID);
            Assert.Equal(product.Name, resultProduct.Name);
        }

        [Fact]
        public async void Create_ActionExecutes_ReturnView()
        {
            var result = _productController.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void CreatePOST_InvalidModelState_ReturnView()
        {
            _productController.ModelState.AddModelError("Name", "Name is required");
            var result = await _productController.Create(products.First());
            var ViewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(ViewResult.Model);
        }

        [Fact]
        public async void CreatePOST_ValidModelState_ReturnRedirectToIndex()
        {
            var result = await _productController.Create(products.First());
            var redirect= Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal<string>("Index", redirect.ActionName);
        }

        [Fact]
        public async void CreatePOST_ValidModelState_CreateMethodExecute()
        {
             Product product = null;
;
            _mockRepo.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x=> product = x);
            var result = await _productController.Create(products.First());  
            _mockRepo.Verify(x=>x.Create(It.IsAny<Product>()),Times.Once);
            Assert.Equal(products.First().ID, product.ID);
        }

        [Fact]
        public async void CreatePOST_InvalidModelState_NeverCreateExecute()
        {
            _productController.ModelState.AddModelError("Name", "");
           // var result = await _productController.Create(products.First());
            _mockRepo.Verify(repo => repo.Create(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async void Edit_IsNull_ReturnRedirectToIndexAction()
        {
           var result = await _productController.Edit(null);
           var redirect= Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(3)]
        public async void Edit_IdInvalid_ReturnNotFound(int productID)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var result = await _productController.Edit(productID);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        public async void Edit_IdValid_ActionExecutes_ReturnProduct(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var result = await _productController.Edit(productID);
            var ViewResult = Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(ViewResult.Model);
            Assert.Equal(product.ID, resultProduct.ID);
            Assert.Equal(product.Name, resultProduct.Name);
        }

        [Theory]
        [InlineData(1)]
        public void EditPOST_IdIsNotEqualProduct_ReturnNotFound(int productID)
        {
            var result = _productController.Edit(2, products.First(x => x.ID == productID));
            var redirect= Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public void EditPOST_InvalidModelState_ReturnView(int productID)
        {
            _productController.ModelState.AddModelError("Name", "");
            var result = _productController.Edit(productID, products.First(x => x.ID == productID));
            var ViewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(ViewResult.Model);
        }

        [Theory]
        [InlineData(1)]
        public void EditPOST_ValidModelState_ReturnRedirectToIndexAction(int productID)
        {
            var result = _productController.Edit(productID, products.First(x => x.ID == productID));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        public void EditPOST_ValidModelState_UpdateMethodExecute(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.Update(product));
            _productController.Edit(productID, product);
            _mockRepo.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async void Delete_IdIsNull_ReturnNotFound()
        {
            var result = await _productController.Delete(null); 
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0)]
        public async void Delete_IdIsNotEqualProduct_ReturnNotFound(int productID)
        {
            Product product = null;
            _mockRepo.Setup(x=>x.GetByID(productID)).ReturnsAsync(product);
            var result = await _productController.Delete(productID);
            Assert.IsType<NotFoundResult>(result);
        }


        [Theory]
        [InlineData(1)]
        public async void Delete_ActionExecutes_ReturnProduct(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup( x => x.GetByID(productID)).ReturnsAsync(product);
            var result = await _productController.Delete(productID);
            var ViewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Product>(ViewResult.Model);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_ReturnRedirectToIndexAction(int productID)
        {
            var result = await _productController.DeleteConfirmed(productID);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_DeleteMethodExecutes(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.Delete(product));
            await _productController.DeleteConfirmed(productID);
            _mockRepo.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        public  void ProductExist_IdIsNull_ReturnFalse(int productID)
        {
            var result = _productController.ProductExists(productID);
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        public void ProductExist_IdIsValid_ReturnTrue(int productID)
        {
            var product = products.First(x => x.ID == productID);
            _mockRepo.Setup(x => x.GetByID(productID)).ReturnsAsync(product);
            var result  = _productController.ProductExists(productID);
            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        public void ProductExist_ActionExecutes_ReturnProduct(int productID)
        {
            var product = products.First(x => x.ID == productID); 
            _mockRepo.Setup(x => x.GetByID(productID)).Returns(Task.FromResult(product));
            var result =  _productController.ProductExists(productID);
            _mockRepo.Verify(x => x.GetByID(productID));
        }

    }
}

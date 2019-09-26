using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void IndexContainsAllProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1", Price = 2},
                new Product {ProductId = 2, Name = "P2", Price = 3},
                new Product {ProductId = 3, Name = "P3", Price = 4},
            }).AsQueryable<Product>());

            AdminController controller = new AdminController(mock.Object);

            var result = (controller.Index() as ViewResult).ViewData.Model
                as IEnumerable<Product>;

            Assert.Equal(3, result.Count());
            Assert.Equal("P2", result.ElementAt(1).Name);
            Assert.Equal(3, result.LastOrDefault().ProductId);
        }

        [Fact]
        public void CanEditProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product {ProductId = 100, Name="P"}
            }).AsQueryable<Product>());

            AdminController controller = new AdminController(mock.Object);
            var result = controller.Edit(100).Model as Product;

            Assert.Equal(100, result.ProductId);
        }

        [Fact]
        public void CannotEditUnexistProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product{ ProductId = 1},
                new Product{ ProductId = 2},
                new Product{ ProductId = 3},
            }).AsQueryable<Product>());

            AdminController controller = new AdminController(mock.Object);

            var result = controller.Edit(5);

            Assert.Null(result.Model);
        }

        [Fact]
        public void CanSaveValidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product { ProductId = 1, Name = "P"}
            }).AsQueryable<Product>());
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController controller = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product { ProductId = 1, Name = "Edited", Price = 5 };

            IActionResult result = controller.Edit(product);

            mock.Verify(m => m.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Fact]
        public void CannotSaveInvalidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { ProductId = 2, Name = "Test", Price = 5 };
            controller.ModelState.AddModelError("error", "error");

            IActionResult result = controller.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }
    }
}
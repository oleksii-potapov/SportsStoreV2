using System;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using System.Collections;
using System.Collections.Generic;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { Name = "P1", ProductId = 1},
                new Product { Name = "P2", ProductId = 2},
                new Product { Name = "P3", ProductId = 3},
                new Product { Name = "P4", ProductId = 4},
                new Product { Name = "P5", ProductId = 5},
            }).AsQueryable<Product>());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            IEnumerable<Product> result =
                controller.List(2).ViewData.Model as IEnumerable<Product>;

            Product[] products = result.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P4", products[0].Name);
            Assert.Equal(5, products[1].ProductId);
        }
    }
}
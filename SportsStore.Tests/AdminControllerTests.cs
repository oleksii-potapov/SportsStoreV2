using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;

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
    }
}
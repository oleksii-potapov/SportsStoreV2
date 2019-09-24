using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanRemoveDuplicatesFromCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product {ProductId = 1, Category = "cat1"},
                new Product {ProductId = 2, Category = "cat1"},
                new Product {ProductId = 3, Category = "cat2"},
                new Product {ProductId = 4, Category = "cat2"},
                new Product {ProductId = 5, Category = "cat2"},
                new Product {ProductId = 6, Category = "cat3"},
            }).AsQueryable<Product>());

            var menu = new NavigationMenuViewComponent(mock.Object);

            var result = (menu.Invoke() as ViewViewComponentResult).ViewData.Model
                as IEnumerable<string>;

            Assert.Equal(3, result.Count());
            Assert.Equal("cat2", result.ElementAt(1));
        }
    }
}
using System;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using System.Collections;
using System.Collections.Generic;
using SportsStore.Models.ViewModels;

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

            var result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            Product[] products = result.Products.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P4", products[0].Name);
            Assert.Equal(5, products[1].ProductId);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            }).AsQueryable<Product>());

            var controller = new ProductController(mock.Object);

            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(4, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void CanFilterCategory()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product {ProductId = 1, Category = "cat1"},
                new Product {ProductId = 2, Category = "cat1"},
                new Product {ProductId = 3, Category = "cat1"},
                new Product {ProductId = 4, Category = "cat2"},
                new Product {ProductId = 5, Category = "cat2"},
            }).AsQueryable<Product>());

            var controller = new ProductController(mock.Object);

            var result = controller.List("cat2").Model as ProductsListViewModel;

            Assert.Equal(2, result.Products.Count());
            Assert.Equal("cat2", result.Products.FirstOrDefault().Category);
        }

        [Fact]
        public void GenerateCategorySpecificProductCount()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new[] {
                new Product {ProductId = 1, Name = "P1", Category="Cat1"},
                new Product {ProductId = 2, Name = "P2", Category="Cat2"},
                new Product {ProductId = 3, Name = "P3", Category="Cat1"},
                new Product {ProductId = 4, Name = "P4", Category="Cat2"},
                new Product {ProductId = 5, Name = "P5", Category="Cat3"},
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            int? res1 = ((ProductsListViewModel)controller.List("Cat1").Model)
                .PagingInfo.TotalItems;
            int? res2 = ((ProductsListViewModel)controller.List("Cat2").Model)
                .PagingInfo.TotalItems;
            int? res3 = ((ProductsListViewModel)controller.List("Cat3").Model)
                .PagingInfo.TotalItems;
            int? resAll = ((ProductsListViewModel)controller.List(null).Model)
                .PagingInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
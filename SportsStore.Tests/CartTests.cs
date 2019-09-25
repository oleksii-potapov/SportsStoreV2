using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void CanCreateCartLineForNewProduct()
        {
            Product product = new Product
            {
                Name = "Apple",
                Category = "Fruits",
                Price = 5
            };

            Cart cart = new Cart();

            cart.AddItem(product, 10);

            Assert.Single(cart.Lines);
        }

        [Fact]
        public void CanAddQuantityForExistingProduct()
        {
            Product p1 = new Product { Name = "Apple", ProductId = 1, Price = 3 };
            Product p2 = new Product { Name = "Orange", ProductId = 2, Price = 1 };

            Cart cart = new Cart();

            cart.AddItem(p1, 10);
            cart.AddItem(p1, 5);
            cart.AddItem(p2, 4);

            Assert.Equal(2, cart.Lines.Count());
            Assert.Equal(15, cart.Lines.ElementAt(0).Quantity);
            Assert.Equal(4, cart.Lines.ElementAt(1).Quantity);
        }

        [Fact]
        public void CanRemoveProductsFromTheCart()
        {
            Product p1 = new Product { ProductId = 1 };
            Product p2 = new Product { ProductId = 2 };

            Cart cart = new Cart();

            cart.AddItem(p1, 13);
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 3);
            cart.RemoveLine(p1);

            Assert.Single(cart.Lines);
            Assert.Equal(2, cart.Lines.ElementAt(0).Product.ProductId);
        }

        [Fact]
        public void CanCalculateTotal()
        {
            Product p1 = new Product { ProductId = 1, Price = 10 };
            Product p2 = new Product { ProductId = 2, Price = 5 };

            Cart cart = new Cart();
            cart.AddItem(p1, 3);
            cart.AddItem(p2, 3);
            decimal total = cart.ComputeTotalValue();

            Assert.Equal(45, total);
        }

        [Fact]
        public void CanResetTheCart()
        {
            Product p1 = new Product { ProductId = 1, Price = 3 };
            Product p2 = new Product { ProductId = 2, Price = 5 };

            Cart cart = new Cart();
            cart.AddItem(p1, 10);
            cart.AddItem(p2, 5);
            cart.Clear();

            Assert.Empty(cart.Lines);
        }
    }
}
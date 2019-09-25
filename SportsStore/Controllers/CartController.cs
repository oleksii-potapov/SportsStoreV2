using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportsStore.Models;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;

        private Cart CurrentCart
        {
            get => HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            set => HttpContext.Session.SetJson("Cart", value);
        }

        public CartController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = CurrentCart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                CurrentCart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                CurrentCart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
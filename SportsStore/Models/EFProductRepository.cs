using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product productToSave = _context.Products
                    .FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productToSave != null)
                {
                    productToSave.Name = product.Name;
                    productToSave.Price = product.Price;
                    productToSave.Description = product.Description;
                    productToSave.Category = product.Category;
                }
            }
            _context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return product;
        }
    }
}
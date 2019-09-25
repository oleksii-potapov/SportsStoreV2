using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> _lines = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = _lines
                .FirstOrDefault(l => l.Product.ProductId == product.ProductId);

            if (line == null)
            {
                _lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            _lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public virtual decimal ComputeTotalValue()
        {
            return _lines.Sum(l => l.Product.Price * l.Quantity);
        }

        public virtual void Clear()
        {
            _lines.Clear();
        }

        public IEnumerable<CartLine> Lines => _lines;
    }
}
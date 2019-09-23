using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    internal interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}
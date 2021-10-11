using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreProductRepository : EFCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
            throw new System.NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                 return context.Products
                                .Where(i=> i.ProductId == id)
                                .Include(i=> i.ProductCategories)
                                .ThenInclude(i=> i.Category)
                                .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                        .Include(i=>i.ProductCategories)
                                        .ThenInclude(i=>i.Category)
                                        .Where(i=>i.ProductCategories.Any(a=>a.Category.Name.ToLower() == name.ToLower()));
                }
                return products.ToList();
            }
        }
    }
}
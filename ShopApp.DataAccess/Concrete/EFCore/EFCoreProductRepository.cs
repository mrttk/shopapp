using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreProductRepository : EFCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i=>i.ProductCategories)
                                    .ThenInclude(i=>i.Category)
                                    .Where(i=>i.ProductCategories.Any(a=>a.Category.Url.ToLower() == category.ToLower()));
                }
                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(i=>i.IsHome && i.IsApproved).ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                 return context.Products
                                .Where(i=> i.Url == url)
                                .Include(i=> i.ProductCategories)
                                .ThenInclude(i=> i.Category)
                                .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                        .Include(i=>i.ProductCategories)
                                        .ThenInclude(i=>i.Category)
                                        .Where(i=>i.ProductCategories.Any(a=>a.Category.Url.ToLower() == name.ToLower()));
                }
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetSearchResult(string searhString)
        {
            using (var context = new ShopContext())
            {
                var products = context
                                    .Products
                                    .Where(i=>i.IsApproved && (i.Name.ToLower().Contains(searhString.ToLower()) || i.Description.ToLower().Contains(searhString.ToLower()))).AsQueryable();
                                    
                return products.ToList();
            }
        }
    }
}
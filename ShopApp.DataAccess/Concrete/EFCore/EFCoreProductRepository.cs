using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class EFCoreProductRepository : EFCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                                .Where(i=>i.ProductId == id)
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .FirstOrDefault();
            }
        }

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

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products.Include(i=>i.ProductCategories)
                                 .FirstOrDefault(i=>i.ProductId == entity.ProductId);
                
                if (product!=null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Url = entity.Url;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;

                    product.ProductCategories = categoryIds.Select(cId=>new ProductCategory()
                    {
                        ProductId = entity.ProductId,
                        CategoryId = cId
                    }).ToList();

                    context.SaveChanges();
                }             
            }
        }
    }
}
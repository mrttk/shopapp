using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                }
                context.SaveChanges();
            }
        }

        private static Category[] Categories = {
            new Category() { Name = "Phones" },
            new Category() { Name = "Computer" },
            new Category() { Name = "Electronic" }
        };
        private static Product[] Products = {
            new Product() { Name = "IPhone 6S", Price = 2000, ImageUrl = "1.jpg", Description = "Good Phone", IsApproved = true },
            new Product() { Name = "IPhone 7", Price = 3000, ImageUrl = "2.jpg", Description = "Good Phone", IsApproved = true },
            new Product() { Name = "IPhone 8", Price = 4000, ImageUrl = "3.jpg", Description = "Good Phone", IsApproved = false },
            new Product() { Name = "IPhone 9", Price = 5000, ImageUrl = "1.jpg", Description = "Better Phone", IsApproved = true }
        };
    }
}
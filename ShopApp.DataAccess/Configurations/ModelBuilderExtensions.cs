using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Configurations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product() { ProductId = 1, Name = "IPhone 6S", Url="iphone-6s",Price = 2000, ImageUrl = "1.jpg", Description = "Good Phone", IsApproved = true },
                new Product() { ProductId = 2, Name = "IPhone 7", Url="iphone-7", Price = 3000, ImageUrl = "2.jpg", Description = "Good Phone", IsApproved = true },
                new Product() { ProductId = 3, Name = "IPhone 8", Url="iphone-8", Price = 4000, ImageUrl = "3.jpg", Description = "Good Phone", IsApproved = false },
                new Product() { ProductId = 4, Name = "IPhone 9", Url="iphone-9",Price = 5000, ImageUrl = "1.jpg", Description = "Better Phone", IsApproved = true }
            );

            builder.Entity<Category>().HasData
            (
                new Category() { CategoryId =1, Name = "Phones", Url = "phones" },
                new Category() { CategoryId =2, Name = "Computer", Url = "computer" },
                new Category() { CategoryId =3, Name = "Electronic", Url = "electronic" }
            );

            builder.Entity<ProductCategory>().HasData(
                new ProductCategory(){ ProductId = 1, CategoryId = 1},
                new ProductCategory(){ ProductId = 1, CategoryId = 2},
                new ProductCategory(){ ProductId = 1, CategoryId = 3},
                new ProductCategory(){ ProductId = 2, CategoryId = 1},
                new ProductCategory(){ ProductId = 2, CategoryId = 2},
                new ProductCategory(){ ProductId = 2, CategoryId = 3},
                new ProductCategory(){ ProductId = 3, CategoryId = 3},
                new ProductCategory(){ ProductId = 4, CategoryId = 3}
            );
        }
    }
}
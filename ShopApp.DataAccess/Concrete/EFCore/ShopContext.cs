using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EFCore
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Data Source=shopDb");
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
            .HasKey(c=> new {c.CategoryId, c.ProductId});
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m=>m.ProductId);
            builder.Property(m=>m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m=>m.CreatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
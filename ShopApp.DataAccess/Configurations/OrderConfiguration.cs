using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(m=>m.Id);
            builder.Property(m=>m.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(m=>m.LastName).IsRequired().HasMaxLength(50);
            builder.Property(m=>m.Email).IsRequired().HasMaxLength(150);
            builder.Property(m=>m.Address).IsRequired().HasMaxLength(500);
            builder.Property(m=>m.City).IsRequired().HasMaxLength(50);
        }
    }
}
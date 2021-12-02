using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(m=>m.CategoryId);
            builder.Property(m=>m.Name).IsRequired().HasMaxLength(50);
            builder.Property(m=>m.Url).IsRequired();
        }
    }
}
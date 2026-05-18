using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Models;

namespace MyApi.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(200);
        builder.Property(c => c.CreatedAt)
            .IsRequired();
        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Devices and gadgets",
                CreatedAt = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                Description = "All kinds of books",
                CreatedAt = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = 3,
                Name = "Clothing",
                Description = "Apparel and accessories",
                CreatedAt = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc)
            }
        );

    }
}
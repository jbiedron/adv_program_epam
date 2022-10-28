using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Amount)
                .IsRequired(true);

            builder.Property(t => t.Price)
                .IsRequired(true);

            builder.Property(t => t.Description)
                .IsRequired(false);

            builder.Property(t => t.Image)
                .IsRequired(false);

        }
    }
}

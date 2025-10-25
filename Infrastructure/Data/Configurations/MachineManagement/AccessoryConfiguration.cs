using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.MachineManagement
{
    public class AccessoryConfiguration : IEntityTypeConfiguration<Accessory>
    {
        public void Configure(EntityTypeBuilder<Accessory> builder)
        {
            builder.ToTable("Accessories");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Category)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.PartNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(a => a.PartNumber)
                .IsUnique();

            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.Property(a => a.Quantity)
                .IsRequired();

            builder.Property(a => a.Unit)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.MinStockLevel)
                .IsRequired();

            builder.Property(a => a.Supplier)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Location)
                .HasMaxLength(200);

            // Relationships
            builder.HasMany(a => a.StockAdjustments)
                .WithOne(sa => sa.Accessory)
                .HasForeignKey(sa => sa.AccessoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

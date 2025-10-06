using Domain.Entities.ExplosiveInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ExplosiveInventory
{
    /// <summary>
    /// EF Core configuration for CentralWarehouseInventory entity
    /// </summary>
    public class CentralWarehouseInventoryConfiguration : IEntityTypeConfiguration<CentralWarehouseInventory>
    {
        public void Configure(EntityTypeBuilder<CentralWarehouseInventory> builder)
        {
            builder.ToTable("CentralWarehouseInventories");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.BatchId)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.BatchId)
                .IsUnique();

            builder.Property(x => x.ExplosiveType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(x => x.AllocatedQuantity)
                .IsRequired()
                .HasPrecision(18, 3)
                .HasDefaultValue(0);

            builder.Property(x => x.Unit)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.ManufacturingDate)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.Supplier)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ManufacturerBatchNumber)
                .HasMaxLength(100);

            builder.Property(x => x.StorageLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            // Audit fields
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(x => x.CentralWarehouse)
                .WithMany()
                .HasForeignKey(x => x.CentralWarehouseStoreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ANFOProperties)
                .WithOne(x => x.Inventory)
                .HasForeignKey<ANFOTechnicalProperties>(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.EmulsionProperties)
                .WithOne(x => x.Inventory)
                .HasForeignKey<EmulsionTechnicalProperties>(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.TransferRequests)
                .WithOne(x => x.CentralInventory)
                .HasForeignKey(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Computed properties - ignored
            builder.Ignore(x => x.AvailableQuantity);
            builder.Ignore(x => x.DaysUntilExpiry);
            builder.Ignore(x => x.IsExpired);
            builder.Ignore(x => x.IsExpiringSoon);

            // Indexes
            builder.HasIndex(x => x.ExplosiveType);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ExpiryDate);
            builder.HasIndex(x => x.Supplier);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}

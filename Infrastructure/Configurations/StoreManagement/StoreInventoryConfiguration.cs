using Domain.Entities.StoreManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.StoreManagement
{
    public class StoreInventoryConfiguration : IEntityTypeConfiguration<StoreInventory>
    {
        public void Configure(EntityTypeBuilder<StoreInventory> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ExplosiveType)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(50);
            
            entity.Property(e => e.Quantity)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.ReservedQuantity)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0);
            
            entity.Property(e => e.Unit)
                  .IsRequired()
                  .HasMaxLength(20);
            
            entity.Property(e => e.MinimumStockLevel)
                  .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.MaximumStockLevel)
                  .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.BatchNumber)
                  .HasMaxLength(100);
            
            entity.Property(e => e.Supplier)
                  .HasMaxLength(200);

            // Relationships
            entity.HasOne(e => e.Store)
                  .WithMany(s => s.Inventories)
                  .HasForeignKey(e => e.StoreId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.ExplosiveType);
            entity.HasIndex(e => new { e.StoreId, e.ExplosiveType })
                  .IsUnique();
            entity.HasIndex(e => e.BatchNumber);
            entity.HasIndex(e => e.ExpiryDate);

            // Check constraints
            entity.HasCheckConstraint("CK_StoreInventory_Quantity_NonNegative", "[Quantity] >= 0");
            entity.HasCheckConstraint("CK_StoreInventory_ReservedQuantity_NonNegative", "[ReservedQuantity] >= 0");
            entity.HasCheckConstraint("CK_StoreInventory_ReservedQuantity_LessOrEqualQuantity", "[ReservedQuantity] <= [Quantity]");
            entity.HasCheckConstraint("CK_StoreInventory_MinMaxStock", "[MinimumStockLevel] IS NULL OR [MaximumStockLevel] IS NULL OR [MinimumStockLevel] <= [MaximumStockLevel]");
        }
    }
}
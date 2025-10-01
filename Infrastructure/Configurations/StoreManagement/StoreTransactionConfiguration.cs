using Domain.Entities.StoreManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.StoreManagement
{
    public class StoreTransactionConfiguration : IEntityTypeConfiguration<StoreTransaction>
    {
        public void Configure(EntityTypeBuilder<StoreTransaction> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ExplosiveType)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(50);
            
            entity.Property(e => e.TransactionType)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);
            
            entity.Property(e => e.Quantity)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Unit)
                  .IsRequired()
                  .HasMaxLength(20);
            
            entity.Property(e => e.ReferenceNumber)
                  .HasMaxLength(100);
            
            entity.Property(e => e.Notes)
                  .HasMaxLength(1000);
            
            entity.Property(e => e.TransactionDate)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(e => e.Store)
                  .WithMany()
                  .HasForeignKey(e => e.StoreId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.StoreInventory)
                  .WithMany()
                  .HasForeignKey(e => e.StoreInventoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.RelatedStore)
                  .WithMany()
                  .HasForeignKey(e => e.RelatedStoreId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.ProcessedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.ProcessedByUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.StoreInventoryId);
            entity.HasIndex(e => e.TransactionType);
            entity.HasIndex(e => e.ExplosiveType);
            entity.HasIndex(e => e.TransactionDate);
            entity.HasIndex(e => e.ReferenceNumber);
            entity.HasIndex(e => e.ProcessedByUserId);
            entity.HasIndex(e => e.RelatedStoreId);

            // Check constraints
            entity.HasCheckConstraint("CK_StoreTransaction_Quantity_Positive", "[Quantity] > 0");
        }
    }
}
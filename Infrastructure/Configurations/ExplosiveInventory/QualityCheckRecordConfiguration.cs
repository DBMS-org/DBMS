using Domain.Entities.ExplosiveInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ExplosiveInventory
{
    /// <summary>
    /// EF Core configuration for QualityCheckRecord entity
    /// </summary>
    public class QualityCheckRecordConfiguration : IEntityTypeConfiguration<QualityCheckRecord>
    {
        public void Configure(EntityTypeBuilder<QualityCheckRecord> builder)
        {
            builder.ToTable("QualityCheckRecords");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CentralWarehouseInventoryId)
                .IsRequired();

            builder.Property(x => x.CheckDate)
                .IsRequired();

            builder.Property(x => x.CheckedByUserId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.CheckType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Findings)
                .HasMaxLength(2000);

            builder.Property(x => x.ActionTaken)
                .HasMaxLength(2000);

            builder.Property(x => x.RequiresFollowUp)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.FollowUpDate);

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
            builder.HasOne(x => x.Inventory)
                .WithMany()
                .HasForeignKey(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CheckedByUser)
                .WithMany()
                .HasForeignKey(x => x.CheckedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.CheckDate);
            builder.HasIndex(x => x.CheckType);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RequiresFollowUp);
        }
    }
}

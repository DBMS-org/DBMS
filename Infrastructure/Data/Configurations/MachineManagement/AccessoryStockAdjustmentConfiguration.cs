using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.MachineManagement
{
    public class AccessoryStockAdjustmentConfiguration : IEntityTypeConfiguration<AccessoryStockAdjustment>
    {
        public void Configure(EntityTypeBuilder<AccessoryStockAdjustment> builder)
        {
            builder.ToTable("AccessoryStockAdjustments");

            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.AccessoryId)
                .IsRequired();

            builder.Property(sa => sa.AdjustmentType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(sa => sa.QuantityChanged)
                .IsRequired();

            builder.Property(sa => sa.PreviousQuantity)
                .IsRequired();

            builder.Property(sa => sa.NewQuantity)
                .IsRequired();

            builder.Property(sa => sa.Reason)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(sa => sa.Notes)
                .HasMaxLength(500);

            builder.Property(sa => sa.AdjustedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(sa => sa.AdjustedDate)
                .IsRequired();

            // Index for querying adjustments by date
            builder.HasIndex(sa => sa.AdjustedDate);

            // Index for querying adjustments by accessory
            builder.HasIndex(sa => sa.AccessoryId);
        }
    }
}

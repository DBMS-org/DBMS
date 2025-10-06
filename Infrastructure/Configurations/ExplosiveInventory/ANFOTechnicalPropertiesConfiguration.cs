using Domain.Entities.ExplosiveInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ExplosiveInventory
{
    /// <summary>
    /// EF Core configuration for ANFOTechnicalProperties entity
    /// </summary>
    public class ANFOTechnicalPropertiesConfiguration : IEntityTypeConfiguration<ANFOTechnicalProperties>
    {
        public void Configure(EntityTypeBuilder<ANFOTechnicalProperties> builder)
        {
            builder.ToTable("ANFOTechnicalProperties");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CentralWarehouseInventoryId)
                .IsRequired();

            // Quality Parameters (Required)
            builder.Property(x => x.Density)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.FuelOilContent)
                .IsRequired()
                .HasPrecision(4, 2);

            // Quality Parameters (Optional)
            builder.Property(x => x.MoistureContent)
                .HasPrecision(4, 2);

            builder.Property(x => x.PrillSize)
                .HasPrecision(4, 2);

            builder.Property(x => x.DetonationVelocity);

            // Manufacturing
            builder.Property(x => x.Grade)
                .IsRequired()
                .HasConversion<int>();

            // Storage Conditions
            builder.Property(x => x.StorageTemperature)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(x => x.StorageHumidity)
                .IsRequired()
                .HasPrecision(5, 2);

            // Quality Control
            builder.Property(x => x.FumeClass)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.QualityCheckDate);

            builder.Property(x => x.QualityStatus)
                .IsRequired()
                .HasConversion<int>();

            // Constants
            builder.Property(x => x.WaterResistance)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("None");

            // Additional
            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Relationship
            builder.HasOne(x => x.Inventory)
                .WithOne(x => x.ANFOProperties)
                .HasForeignKey<ANFOTechnicalProperties>(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

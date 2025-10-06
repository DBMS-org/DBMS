using Domain.Entities.ExplosiveInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ExplosiveInventory
{
    /// <summary>
    /// EF Core configuration for EmulsionTechnicalProperties entity
    /// </summary>
    public class EmulsionTechnicalPropertiesConfiguration : IEntityTypeConfiguration<EmulsionTechnicalProperties>
    {
        public void Configure(EntityTypeBuilder<EmulsionTechnicalProperties> builder)
        {
            builder.ToTable("EmulsionTechnicalProperties");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CentralWarehouseInventoryId)
                .IsRequired();

            // Density (Required)
            builder.Property(x => x.DensityUnsensitized)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.DensitySensitized)
                .IsRequired()
                .HasPrecision(4, 2);

            // Rheological (Required)
            builder.Property(x => x.Viscosity)
                .IsRequired();

            // Composition (Required)
            builder.Property(x => x.WaterContent)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.pH)
                .IsRequired()
                .HasPrecision(3, 1);

            // Performance (Optional)
            builder.Property(x => x.DetonationVelocity);

            builder.Property(x => x.BubbleSize);

            // Temperature
            builder.Property(x => x.StorageTemperature)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(x => x.ApplicationTemperature)
                .HasPrecision(5, 2);

            // Manufacturing
            builder.Property(x => x.Grade)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(50);

            // Sensitization
            builder.Property(x => x.SensitizationType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.SensitizerContent)
                .HasPrecision(5, 2);

            // Quality Control
            builder.Property(x => x.FumeClass)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.QualityCheckDate);

            builder.Property(x => x.QualityStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.PhaseSeparation);

            builder.Property(x => x.Crystallization);

            builder.Property(x => x.ColorConsistency);

            // Constants
            builder.Property(x => x.WaterResistance)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Excellent");

            // Additional
            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Relationship
            builder.HasOne(x => x.Inventory)
                .WithOne(x => x.EmulsionProperties)
                .HasForeignKey<EmulsionTechnicalProperties>(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

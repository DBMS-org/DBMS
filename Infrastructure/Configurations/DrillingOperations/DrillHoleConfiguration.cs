using Domain.Entities.DrillingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class DrillHoleConfiguration : IEntityTypeConfiguration<DrillHole>
    {
        public void Configure(EntityTypeBuilder<DrillHole> entity)
        {
            // Set Id as the primary key (string)
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired().HasMaxLength(450); // Reasonable limit for GUID strings
            
            // Configure SerialNumber as a regular property (not primary key, not auto-increment)
            entity.Property(e => e.SerialNumber).IsRequired(false);
            
            entity.Property(e => e.ProjectId).IsRequired();
            entity.Property(e => e.SiteId).IsRequired();
            entity.Property(e => e.Depth).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Length).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Elevation).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Easting).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Northing).HasColumnType("decimal(10,2)");

            // A DrillHole currently does not have navigations; consider future mapping
        }
    }
}
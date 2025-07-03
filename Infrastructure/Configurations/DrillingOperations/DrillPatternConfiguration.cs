using Domain.Entities.DrillingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class DrillPatternConfiguration : IEntityTypeConfiguration<DrillPattern>
    {
        public void Configure(EntityTypeBuilder<DrillPattern> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DrillPointsJson).IsRequired();

            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
            entity.HasIndex(e => e.IsActive);

            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 
using Domain.Entities.DrillingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class DrillPointConfiguration : IEntityTypeConfiguration<DrillPoint>
    {
        public void Configure(EntityTypeBuilder<DrillPoint> entity)
        {
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });
            entity.Property(e => e.Id).IsRequired();

            entity.Property(e => e.X).HasPrecision(18, 2);
            entity.Property(e => e.Y).HasPrecision(18, 2);
            entity.Property(e => e.Depth).HasPrecision(18, 2);
            entity.Property(e => e.Spacing).HasPrecision(18, 2);
            entity.Property(e => e.Burden).HasPrecision(18, 2);

            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
        }
    }
} 
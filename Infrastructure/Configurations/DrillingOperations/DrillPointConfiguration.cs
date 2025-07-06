using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.DrillingOperations;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class DrillPointConfiguration : IEntityTypeConfiguration<DrillPoint>
    {
        public void Configure(EntityTypeBuilder<DrillPoint> entity)
        {
            entity.ToTable("DrillPoints");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .IsRequired();
                
            entity.Property(e => e.X)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.Y)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.Depth)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.Spacing)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.Burden)
                .HasPrecision(18, 6)
                .IsRequired();

            // Configure relationships to Project and Site
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
            entity.HasIndex(e => new { e.X, e.Y, e.ProjectId, e.SiteId });
        }
    }
} 
using Domain.Entities.BlastingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.BlastingOperations
{
    public class BlastConnectionConfiguration : IEntityTypeConfiguration<BlastConnection>
    {
        public void Configure(EntityTypeBuilder<BlastConnection> entity)
        {
            entity.ToTable("BlastConnections");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired().HasMaxLength(450);
            
            entity.Property(e => e.Point1DrillPointId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.Point2DrillPointId).IsRequired().HasMaxLength(450);
            
            entity.Property(e => e.ConnectorType).IsRequired();
            entity.Property(e => e.Delay).IsRequired();
            entity.Property(e => e.Sequence).IsRequired();
            
            entity.Property(e => e.ProjectId).IsRequired();
            entity.Property(e => e.SiteId).IsRequired();
            
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            
            // Foreign key relationships
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Point1DrillPoint)
                .WithMany()
                .HasForeignKey(e => new { e.Point1DrillPointId, e.ProjectId, e.SiteId })
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Point2DrillPoint)
                .WithMany()
                .HasForeignKey(e => new { e.Point2DrillPointId, e.ProjectId, e.SiteId })
                .OnDelete(DeleteBehavior.Restrict);
            
            // Indexes for performance
            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
            entity.HasIndex(e => e.Sequence);
        }
    }

    public class DetonatorInfoConfiguration : IEntityTypeConfiguration<DetonatorInfo>
    {
        public void Configure(EntityTypeBuilder<DetonatorInfo> entity)
        {
            entity.ToTable("DetonatorInfos");
            
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });
            
            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .IsRequired();
                
            entity.Property(e => e.DrillPointId)
                .HasMaxLength(450)
                .IsRequired();

            // Foreign key relationships
            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.DrillPoint)
                  .WithMany()
                  .HasForeignKey(e => new { e.DrillPointId, e.ProjectId, e.SiteId })
                  .OnDelete(DeleteBehavior.Restrict);
                  
            // Indexes for performance
            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
            entity.HasIndex(e => e.DrillPointId);
        }
    }
}
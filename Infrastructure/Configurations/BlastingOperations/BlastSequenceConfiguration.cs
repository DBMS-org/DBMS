using Domain.Entities.BlastingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.BlastingOperations
{
    public class BlastSequenceConfiguration : IEntityTypeConfiguration<BlastSequence>
    {
        public void Configure(EntityTypeBuilder<BlastSequence> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ConnectionsJson).IsRequired();
            entity.Property(e => e.SimulationSettingsJson).HasDefaultValue("{}");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId });

            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.DrillPattern)
                  .WithMany(dp => dp.BlastSequences)
                  .HasForeignKey(e => e.DrillPatternId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 
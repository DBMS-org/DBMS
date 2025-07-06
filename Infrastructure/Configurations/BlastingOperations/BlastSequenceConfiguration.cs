using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.BlastingOperations;

namespace Infrastructure.Configurations.BlastingOperations
{
    public class BlastSequenceConfiguration : IEntityTypeConfiguration<BlastSequence>
    {
        public void Configure(EntityTypeBuilder<BlastSequence> entity)
        {
            entity.ToTable("BlastSequences");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();
                
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
                
            entity.Property(e => e.SimulationSettingsJson)
                .IsRequired();

            // Configure relationships
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => new { e.ProjectId, e.SiteId });
            entity.HasIndex(e => e.IsActive);
        }
    }
} 
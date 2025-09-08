using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.DrillingOperations;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class ExplosiveCalculationResultConfiguration : IEntityTypeConfiguration<ExplosiveCalculationResult>
    {
        public void Configure(EntityTypeBuilder<ExplosiveCalculationResult> entity)
        {
            entity.ToTable("ExplosiveCalculationResults");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.CalculationId)
                .HasMaxLength(450)
                .IsRequired();
                
            // Create unique index on CalculationId
            entity.HasIndex(e => e.CalculationId)
                .IsUnique();
            
            // Explosive material properties
            entity.Property(e => e.EmulsionDensity)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.AnfoDensity)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.EmulsionPerHole)
                .HasPrecision(18, 6)
                .IsRequired();
            
            // Calculation results with high precision
            entity.Property(e => e.TotalDepth)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.AverageDepth)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.NumberOfFilledHoles)
                .IsRequired();
                
            entity.Property(e => e.EmulsionPerMeter)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.AnfoPerMeter)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.EmulsionCoveringSpace)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.RemainingSpace)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.AnfoCoveringSpace)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.TotalAnfo)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.TotalEmulsion)
                .HasPrecision(18, 6)
                .IsRequired();
                
            entity.Property(e => e.TotalVolume)
                .HasPrecision(18, 6)
                .IsRequired();
            
            // Foreign key relationships
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.PatternSettings)
                .WithMany()
                .HasForeignKey(e => e.PatternSettingsId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure foreign key relationships
            entity.HasOne(e => e.OwningUser)
                  .WithMany()
                  .HasForeignKey(e => e.OwningUserId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            // Composite index for performance
            entity.HasIndex(e => new { e.ProjectId, e.SiteId, e.CreatedAt })
                .HasDatabaseName("IX_ExplosiveCalculationResults_ProjectSite_CreatedAt");
            
            // Unique constraint to enforce one calculation per site
            entity.HasIndex(e => new { e.ProjectId, e.SiteId })
                .IsUnique()
                .HasDatabaseName("IX_ExplosiveCalculationResults_ProjectSite_Unique");
        }
    }
}
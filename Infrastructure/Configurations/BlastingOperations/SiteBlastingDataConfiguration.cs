using Domain.Entities.BlastingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SiteBlastingDataConfiguration : IEntityTypeConfiguration<SiteBlastingData>
    {
        public void Configure(EntityTypeBuilder<SiteBlastingData> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DataType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.JsonData).IsRequired();

            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Site)
                  .WithMany()
                  .HasForeignKey(e => e.SiteId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.CreatedBy)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ProjectId, e.SiteId, e.DataType }).IsUnique();
        }
    }
} 
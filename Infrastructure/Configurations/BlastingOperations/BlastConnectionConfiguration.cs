using Domain.Entities.BlastingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.BlastingOperations
{
    public class BlastConnectionConfiguration : IEntityTypeConfiguration<BlastConnection>
    {
        public void Configure(EntityTypeBuilder<BlastConnection> entity)
        {
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });

            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.FromHoleId).IsRequired();
            entity.Property(e => e.ToHoleId).IsRequired();
            entity.Property(e => e.StartPointJson).IsRequired();
            entity.Property(e => e.EndPointJson).IsRequired();

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

    public class DetonatorInfoConfiguration : IEntityTypeConfiguration<DetonatorInfo>
    {
        public void Configure(EntityTypeBuilder<DetonatorInfo> entity)
        {
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.HoleId).IsRequired();

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
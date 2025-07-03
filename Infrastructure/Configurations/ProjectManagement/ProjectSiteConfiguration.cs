using Domain.Common;
using Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ProjectManagement
{
    public class ProjectSiteConfiguration : IEntityTypeConfiguration<ProjectSite>
    {
        public void Configure(EntityTypeBuilder<ProjectSite> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Coordinates).HasMaxLength(200);
            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(e => e.Project)
                  .WithMany(p => p.ProjectSites)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProjectId);
        }
    }
} 
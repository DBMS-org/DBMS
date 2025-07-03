using Domain.Common;
using Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ProjectManagement
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Region).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(e => e.AssignedUser)
                  .WithMany()
                  .HasForeignKey(e => e.AssignedUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.RegionNavigation)
                  .WithMany(r => r.Projects)
                  .HasForeignKey(e => e.RegionId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.RegionId);
            entity.HasIndex(e => e.AssignedUserId)
                  .IsUnique()
                  .HasFilter("[AssignedUserId] IS NOT NULL");
        }
    }
} 
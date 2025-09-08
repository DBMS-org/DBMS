using Domain.Entities.DrillingOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.DrillingOperations
{
    public class PatternSettingsConfiguration : IEntityTypeConfiguration<PatternSettings>
    {
        public void Configure(EntityTypeBuilder<PatternSettings> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Spacing).HasPrecision(18, 6).HasDefaultValue(3.0);
            entity.Property(e => e.Burden).HasPrecision(18, 6).HasDefaultValue(2.5);
            entity.Property(e => e.Depth).HasPrecision(18, 6).HasDefaultValue(10.0);
            entity.Property(e => e.Diameter).HasPrecision(18, 6).HasDefaultValue(0.89);
            entity.Property(e => e.Stemming).HasPrecision(18, 6).HasDefaultValue(3.0);

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
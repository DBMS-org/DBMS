using Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ExplosiveApprovalRequestConfiguration : IEntityTypeConfiguration<ExplosiveApprovalRequest>
    {
        public void Configure(EntityTypeBuilder<ExplosiveApprovalRequest> builder)
        {
            builder.ToTable("ExplosiveApprovalRequests");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            builder.Property(e => e.ExpectedUsageDate)
                .IsRequired();
            
            builder.Property(e => e.Comments)
                .HasMaxLength(1000);
            
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();
            
            builder.Property(e => e.Priority)
                .IsRequired()
                .HasConversion<int>();
            
            builder.Property(e => e.ApprovalType)
                .IsRequired()
                .HasConversion<int>();
            
            builder.Property(e => e.RejectionReason)
                .HasMaxLength(500);
            
            builder.Property(e => e.AdditionalData)
                .HasColumnType("nvarchar(max)");
            
            builder.Property(e => e.EstimatedDurationHours)
                .HasPrecision(18, 2);
            
            builder.Property(e => e.SafetyChecklistCompleted)
                .IsRequired()
                .HasDefaultValue(false);
            
            builder.Property(e => e.EnvironmentalAssessmentCompleted)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Relationships
            builder.HasOne(e => e.ProjectSite)
                .WithMany(ps => ps.ExplosiveApprovalRequests)
                .HasForeignKey(e => e.ProjectSiteId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(e => e.RequestedByUser)
                .WithMany()
                .HasForeignKey(e => e.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(e => e.ProcessedByUser)
                .WithMany()
                .HasForeignKey(e => e.ProcessedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            
            // Indexes for performance
            builder.HasIndex(e => e.ProjectSiteId)
                .HasDatabaseName("IX_ExplosiveApprovalRequests_ProjectSiteId");
            
            builder.HasIndex(e => e.RequestedByUserId)
                .HasDatabaseName("IX_ExplosiveApprovalRequests_RequestedByUserId");
            
            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_ExplosiveApprovalRequests_Status");
            
            builder.HasIndex(e => e.ExpectedUsageDate)
                .HasDatabaseName("IX_ExplosiveApprovalRequests_ExpectedUsageDate");
            
            builder.HasIndex(e => new { e.ProjectSiteId, e.Status })
                .HasDatabaseName("IX_ExplosiveApprovalRequests_ProjectSite_Status");
        }
    }
}
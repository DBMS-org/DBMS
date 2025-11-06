using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.BlastingOperations
{
    public class SiteBlastingData
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public int SiteId { get; set; }
        
        public string DataType { get; set; } = string.Empty;

        public string JsonData { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int CreatedByUserId { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;

        [NotMapped]
        public string CompositeKey => $"{ProjectId}_{SiteId}_{DataType}";
    }
} 

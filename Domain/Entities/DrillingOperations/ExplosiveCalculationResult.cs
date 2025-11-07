using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.DrillingOperations
{
    public class ExplosiveCalculationResult : BaseAuditableEntity, IEntityOwnedByUser
    {
        public string CalculationId { get; set; } = string.Empty;
        
        // Reference to pattern settings used for calculation
        public int? PatternSettingsId { get; set; }
        
        // Explosive material properties (not available in existing entities)
        public double EmulsionDensity { get; set; }
        public double AnfoDensity { get; set; }
        public double EmulsionPerHole { get; set; }
        
        // Calculated results
        public double TotalDepth { get; set; }
        public double AverageDepth { get; set; }
        public int NumberOfFilledHoles { get; set; }
        public double EmulsionPerMeter { get; set; }
        public double AnfoPerMeter { get; set; }
        public double EmulsionCoveringSpace { get; set; }
        public double RemainingSpace { get; set; }
        public double AnfoCoveringSpace { get; set; }
        public double TotalAnfo { get; set; }
        public double TotalEmulsion { get; set; }
        public double TotalVolume { get; set; }
        
        // Project and Site context (inherited from pattern or drill points)
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        // User ownership for audit trail
        public int OwningUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual PatternSettings? PatternSettings { get; set; }
        public virtual User OwningUser { get; set; } = null!;
    }
}
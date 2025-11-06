using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.DrillingOperations
{
    public class ExplosiveCalculationResult : BaseAuditableEntity, IEntityOwnedByUser
    {
        public string CalculationId { get; set; } = string.Empty;

        public int? PatternSettingsId { get; set; }

        public double EmulsionDensity { get; set; }
        public double AnfoDensity { get; set; }
        public double EmulsionPerHole { get; set; }

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

        public int ProjectId { get; set; }
        public int SiteId { get; set; }

        public int OwningUserId { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual PatternSettings? PatternSettings { get; set; }
        public virtual User OwningUser { get; set; } = null!;
    }
}
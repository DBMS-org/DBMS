namespace Domain.Entities
{
    public class ProjectSite : BaseEntity
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public bool IsPatternApproved { get; set; } = false;
        public bool IsSimulationConfirmed { get; set; } = false;
        public bool IsOperatorCompleted { get; set; } = false;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<DrillPattern> DrillPatterns { get; set; } = new List<DrillPattern>();
        public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();
        public virtual ICollection<SiteBlastingData> SiteBlastingData { get; set; } = new List<SiteBlastingData>();
        public virtual ICollection<DrillHole> DrillHoles { get; set; } = new List<DrillHole>();
        
        // Business logic methods
        public bool IsActive() => Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
        
        public bool IsCompleted() => Status.Equals("Completed", StringComparison.OrdinalIgnoreCase);
        
        public void ApprovePattern()
        {
            IsPatternApproved = true;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void ConfirmSimulation()
        {
            IsSimulationConfirmed = true;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void CompleteOperatorWork()
        {
            IsOperatorCompleted = true;
            if (IsPatternApproved && IsSimulationConfirmed)
            {
                Status = "Completed";
            }
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public double GetCompletionPercentage()
        {
            int completedSteps = 0;
            if (IsPatternApproved) completedSteps++;
            if (IsSimulationConfirmed) completedSteps++;
            if (IsOperatorCompleted) completedSteps++;
            
            return (completedSteps / 3.0) * 100.0;
        }
    }
} 



namespace Domain.Entities
{
    public class ProjectSite
    {
        public int Id { get; set; }
        

        public int ProjectId { get; set; }
        


        public string Name { get; set; } = string.Empty;
        

        public string Location { get; set; } = string.Empty;
        

        public string Coordinates { get; set; } = string.Empty;
        


        public string Status { get; set; } = string.Empty;
        

        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsPatternApproved { get; set; } = false;
        public bool IsSimulationConfirmed { get; set; } = false;
        public bool IsOperatorCompleted { get; set; } = false;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
    }
} 

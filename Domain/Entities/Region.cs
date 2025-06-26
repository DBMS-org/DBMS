namespace Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
        
        // Business logic methods
        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public int GetActiveProjectsCount()
        {
            return Projects.Count(p => p.IsActive());
        }
        
        public int GetActiveMachinesCount()
        {
            return Machines.Count(m => m.IsAvailable() || m.IsInUse());
        }
    }
} 

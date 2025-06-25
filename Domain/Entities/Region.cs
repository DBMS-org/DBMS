

namespace Domain.Entities
{
    public class Region
    {
        public int Id { get; set; }
        


        public string Name { get; set; } = string.Empty;
        

        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
    }
} 

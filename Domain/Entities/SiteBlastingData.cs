using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class SiteBlastingData : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string DataType { get; set; } = string.Empty; // "pattern", "connections", "simulation_settings", "simulation_state"
        public string JsonData { get; set; } = string.Empty; // Serialized JSON data
        
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        
        // Add indexes for better performance
        public string CompositeKey => $"{ProjectId}_{SiteId}_{DataType}";
        
        // Business logic methods
        public void UpdateData(string jsonData)
        {
            JsonData = jsonData;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public bool HasData()
        {
            return !string.IsNullOrEmpty(JsonData) && JsonData != "{}" && JsonData != "[]";
        }
        
        public bool IsPatternData() => DataType.Equals("pattern", StringComparison.OrdinalIgnoreCase);
        public bool IsConnectionsData() => DataType.Equals("connections", StringComparison.OrdinalIgnoreCase);
        public bool IsSimulationSettingsData() => DataType.Equals("simulation_settings", StringComparison.OrdinalIgnoreCase);
        public bool IsSimulationStateData() => DataType.Equals("simulation_state", StringComparison.OrdinalIgnoreCase);
    }
} 

namespace Domain.Entities
{
    public class BlastSequence : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public int DrillPatternId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Blast connections as JSON
        public string ConnectionsJson { get; set; } = string.Empty;
        
        // Simulation settings as JSON
        public string SimulationSettingsJson { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPattern DrillPattern { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        
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
        
        public void UpdateConnections(string connectionsJson)
        {
            ConnectionsJson = connectionsJson;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void UpdateSimulationSettings(string simulationSettingsJson)
        {
            SimulationSettingsJson = simulationSettingsJson;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public bool HasConnections()
        {
            return !string.IsNullOrEmpty(ConnectionsJson) && ConnectionsJson != "[]";
        }
        
        public bool HasSimulationSettings()
        {
            return !string.IsNullOrEmpty(SimulationSettingsJson) && SimulationSettingsJson != "{}";
        }
    }
} 

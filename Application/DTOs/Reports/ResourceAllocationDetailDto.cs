namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed resource allocation information for reports
    /// </summary>
    public class ResourceAllocationDetailDto
    {
        public string ResourceType { get; set; } = string.Empty; // Machine, Operator, Engineer, Manager
        public int TotalResources { get; set; }
        public int AvailableResources { get; set; }
        public int AssignedResources { get; set; }
        public decimal UtilizationRate { get; set; }
        public List<RegionalResourceAllocationDto> ByRegion { get; set; } = new();
        public List<ProjectResourceDto> ByProject { get; set; } = new();
    }

    public class RegionalResourceAllocationDto
    {
        public string RegionName { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Available { get; set; }
        public int Assigned { get; set; }
    }

    public class ProjectResourceDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public int ResourcesAssigned { get; set; }
        public string ResourceType { get; set; } = string.Empty;
    }
}

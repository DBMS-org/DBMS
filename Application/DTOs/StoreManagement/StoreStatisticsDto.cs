namespace Application.DTOs.StoreManagement
{
    public class StoreStatisticsDto
    {
        public int TotalStores { get; set; }
        public int ActiveStores { get; set; }
        public int InactiveStores { get; set; }
        public int OperationalStores { get; set; }
        public int MaintenanceStores { get; set; }
        public decimal TotalCapacity { get; set; }
        public decimal TotalOccupancy { get; set; }
        public decimal UtilizationRate { get; set; }
        public Dictionary<string, int> StoresByRegion { get; set; } = new Dictionary<string, int>();
    }
}
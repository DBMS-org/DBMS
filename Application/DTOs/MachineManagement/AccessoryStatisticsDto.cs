namespace Application.DTOs.MachineManagement
{
    public class AccessoryStatisticsDto
    {
        public int TotalAvailable { get; set; }
        public int LowStock { get; set; }
        public int OutOfStock { get; set; }
        public int TotalItems { get; set; }
    }
}

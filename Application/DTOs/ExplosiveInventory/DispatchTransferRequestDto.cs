namespace Application.DTOs.ExplosiveInventory
{
    public class DispatchTransferRequestDto
    {
        public string TruckNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string? DriverContactNumber { get; set; }
        public string? DispatchNotes { get; set; }
    }
}

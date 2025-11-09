namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for dispatching a transfer request
    /// </summary>
    public class DispatchTransferRequestDto
    {
        public string TruckNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string? DriverContactNumber { get; set; }
        public string? DispatchNotes { get; set; }
    }
}

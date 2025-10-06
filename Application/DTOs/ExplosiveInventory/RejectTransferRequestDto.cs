namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for rejecting transfer request
    /// </summary>
    public class RejectTransferRequestDto
    {
        public string RejectionReason { get; set; } = string.Empty;
    }
}

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for approving transfer request
    /// </summary>
    public class ApproveTransferRequestDto
    {
        public decimal? ApprovedQuantity { get; set; }
        public string? ApprovalNotes { get; set; }
    }
}

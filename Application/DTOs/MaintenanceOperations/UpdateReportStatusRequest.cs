using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MaintenanceOperations
{
    public class UpdateReportStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}

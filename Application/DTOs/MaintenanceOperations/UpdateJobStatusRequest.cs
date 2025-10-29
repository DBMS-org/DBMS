using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MaintenanceOperations
{
    public class UpdateJobStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}

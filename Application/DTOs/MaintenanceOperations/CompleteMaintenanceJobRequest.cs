using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MaintenanceOperations
{
    public class CompleteMaintenanceJobRequest
    {
        [Required]
        [MinLength(20, ErrorMessage = "Observations must be at least 20 characters")]
        public string Observations { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000, ErrorMessage = "Actual hours must be between 0.1 and 1000")]
        public decimal ActualHours { get; set; }

        public List<string>? PartsReplaced { get; set; }
    }
}

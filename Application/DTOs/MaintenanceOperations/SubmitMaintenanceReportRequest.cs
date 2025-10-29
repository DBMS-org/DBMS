using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MaintenanceOperations
{
    public class SubmitMaintenanceReportRequest
    {
        [Required]
        public int OperatorId { get; set; }

        [Required]
        public int MachineId { get; set; }

        [Required]
        public string AffectedPart { get; set; } = string.Empty;

        [Required]
        public string ProblemCategory { get; set; } = string.Empty;

        [Required]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        [MaxLength(2000)]
        public string CustomDescription { get; set; } = string.Empty;

        public List<string>? Symptoms { get; set; }

        [MaxLength(500)]
        public string? ErrorCodes { get; set; }

        [Required]
        public string Severity { get; set; } = string.Empty;
    }
}

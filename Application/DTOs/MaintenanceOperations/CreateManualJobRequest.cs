using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MaintenanceOperations
{
    public class CreateManualJobRequest
    {
        [Required]
        public int MachineId { get; set; }

        public int? ProjectId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000)]
        public decimal EstimatedHours { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public int? AssignedEngineerId { get; set; }
    }
}

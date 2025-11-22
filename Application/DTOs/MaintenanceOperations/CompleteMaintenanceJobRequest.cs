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

        // Service completion flags (legacy flag kept for backward compatibility)
        public bool IsServiceCompleted { get; set; }

        // Specific service completion flags - reset respective service hours when true
        public bool IsEngineServiceCompleted { get; set; }
        public bool IsDrifterServiceCompleted { get; set; }

        // Consumed drilling components
        public int DrillBitsUsed { get; set; }
        public string? DrillBitType { get; set; }
        public int DrillRodsUsed { get; set; }
        public string? DrillRodType { get; set; }
        public int ShanksUsed { get; set; }
        public string? ShankType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class CreateMachineAssignmentDto
    {
        [Required]
        public int MachineId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int OperatorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AssignedBy { get; set; } = string.Empty;

        public DateTime? ExpectedReturnDate { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}

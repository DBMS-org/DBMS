using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class ApproveAssignmentRequestDto
    {
        [Required]
        public List<int> AssignedMachines { get; set; } = new();

        [MaxLength(500)]
        public string? Comments { get; set; }
    }
}

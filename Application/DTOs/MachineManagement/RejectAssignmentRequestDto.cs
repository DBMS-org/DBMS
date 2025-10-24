using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class RejectAssignmentRequestDto
    {
        [Required]
        [MaxLength(500)]
        public string Comments { get; set; } = string.Empty;
    }
}

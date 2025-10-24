using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class CreateAssignmentRequestDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MachineType { get; set; } = string.Empty;

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(100)]
        public string RequestedBy { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Urgency { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? DetailsOrExplanation { get; set; }

        [MaxLength(100)]
        public string? ExpectedUsageDuration { get; set; }

        public DateTime? ExpectedReturnDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.DrillingOperations
{
    public class ExplosiveCalculationResultDto
    {
        public int Id { get; set; }
        public string CalculationId { get; set; } = string.Empty;
        public int? PatternSettingsId { get; set; }

        public double EmulsionDensity { get; set; }
        public double AnfoDensity { get; set; }
        public double EmulsionPerHole { get; set; }

        public double TotalDepth { get; set; }
        public double AverageDepth { get; set; }
        public int NumberOfFilledHoles { get; set; }
        public double EmulsionPerMeter { get; set; }
        public double AnfoPerMeter { get; set; }
        public double EmulsionCoveringSpace { get; set; }
        public double RemainingSpace { get; set; }
        public double AnfoCoveringSpace { get; set; }
        public double TotalAnfo { get; set; }
        public double TotalEmulsion { get; set; }
        public double TotalVolume { get; set; }

        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public int OwningUserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
    
    public class CreateExplosiveCalculationResultRequest
    {
        [Required]
        [StringLength(450)]
        public string CalculationId { get; set; } = string.Empty;

        public int? PatternSettingsId { get; set; }

        [Required]
        [Range(0.1, 10.0, ErrorMessage = "Emulsion density must be between 0.1 and 10.0")]
        public double EmulsionDensity { get; set; }
        
        [Required]
        [Range(0.1, 10.0, ErrorMessage = "ANFO density must be between 0.1 and 10.0")]
        public double AnfoDensity { get; set; }
        
        [Required]
        [Range(0.1, 1000.0, ErrorMessage = "Emulsion per hole must be between 0.1 and 1000.0")]
        public double EmulsionPerHole { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total depth must be non-negative")]
        public double TotalDepth { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Average depth must be non-negative")]
        public double AverageDepth { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of filled holes must be non-negative")]
        public int NumberOfFilledHoles { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Emulsion per meter must be non-negative")]
        public double EmulsionPerMeter { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "ANFO per meter must be non-negative")]
        public double AnfoPerMeter { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Emulsion covering space must be non-negative")]
        public double EmulsionCoveringSpace { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Remaining space must be non-negative")]
        public double RemainingSpace { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "ANFO covering space must be non-negative")]
        public double AnfoCoveringSpace { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total ANFO must be non-negative")]
        public double TotalAnfo { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total emulsion must be non-negative")]
        public double TotalEmulsion { get; set; }
        
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total volume must be non-negative")]
        public double TotalVolume { get; set; }

        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        public int OwningUserId { get; set; }
    }
    
    public class UpdateExplosiveCalculationResultRequest
    {
        [StringLength(450)]
        public string? CalculationId { get; set; }

        public int? PatternSettingsId { get; set; }

        [Range(0.1, 10.0, ErrorMessage = "Emulsion density must be between 0.1 and 10.0")]
        public double? EmulsionDensity { get; set; }
        
        [Range(0.1, 10.0, ErrorMessage = "ANFO density must be between 0.1 and 10.0")]
        public double? AnfoDensity { get; set; }
        
        [Range(0.1, 1000.0, ErrorMessage = "Emulsion per hole must be between 0.1 and 1000.0")]
        public double? EmulsionPerHole { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Total depth must be non-negative")]
        public double? TotalDepth { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Average depth must be non-negative")]
        public double? AverageDepth { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Number of filled holes must be non-negative")]
        public int? NumberOfFilledHoles { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Emulsion per meter must be non-negative")]
        public double? EmulsionPerMeter { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "ANFO per meter must be non-negative")]
        public double? AnfoPerMeter { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Emulsion covering space must be non-negative")]
        public double? EmulsionCoveringSpace { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Remaining space must be non-negative")]
        public double? RemainingSpace { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "ANFO covering space must be non-negative")]
        public double? AnfoCoveringSpace { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Total ANFO must be non-negative")]
        public double? TotalAnfo { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Total emulsion must be non-negative")]
        public double? TotalEmulsion { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "Total volume must be non-negative")]
        public double? TotalVolume { get; set; }
    }
}
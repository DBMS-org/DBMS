namespace Application.DTOs.MachineManagement
{
    public class MachineSpecificationsDto
    {
        public string? Power { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Capacity { get; set; }
        public string? OperatingTemperature { get; set; }
        public string? FuelType { get; set; }
        public string? MaxOperatingDepth { get; set; }
        public string? DrillingDiameter { get; set; }
        public List<string>? AdditionalFeatures { get; set; }
    }
} 
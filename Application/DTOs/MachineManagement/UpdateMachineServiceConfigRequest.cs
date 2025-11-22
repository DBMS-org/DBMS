namespace Application.DTOs.MachineManagement
{
    public class UpdateMachineServiceConfigRequest
    {
        public decimal EngineServiceInterval { get; set; }
        public decimal? DrifterServiceInterval { get; set; }
    }
}

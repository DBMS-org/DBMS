using AutoMapper;
using Application.DTOs.MachineManagement;
using Domain.Entities.MachineManagement;
using System.Text.Json;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Machine Management mappings
    /// </summary>
    public class MachineManagementMappingProfile : Profile
    {
        public MachineManagementMappingProfile()
        {
            // Machine mappings
            CreateMap<Machine, MachineDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateMachineRequest, Machine>()
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Operator, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());

            CreateMap<UpdateMachineRequest, Machine>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Operator, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());

            // MachineSpecifications mappings
        }

        private static MachineSpecificationsDto DeserializeMachineSpecificationsDto(string src)
        {
            try
            {
                return JsonSerializer.Deserialize<MachineSpecificationsDto>(src) ?? new MachineSpecificationsDto();
            }
            catch
            {
                return new MachineSpecificationsDto();
            }
        }
    }

    public class MachineStatusResolver : IValueResolver<UpdateMachineRequest, Machine, MachineStatus>
    {
        public MachineStatus Resolve(UpdateMachineRequest source, Machine destination, MachineStatus destMember, ResolutionContext context)
        {
            return Enum.TryParse<MachineStatus>(source.Status, out var result) ? result : MachineStatus.Available;
        }
    }
} 
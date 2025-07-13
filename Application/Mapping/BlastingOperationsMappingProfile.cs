using AutoMapper;
using Application.DTOs.BlastingOperations;
using Domain.Entities.BlastingOperations;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Blasting Operations mappings
    /// </summary>
    public class BlastingOperationsMappingProfile : Profile
    {
        public BlastingOperationsMappingProfile()
        {
            // BlastConnection mappings
            CreateMap<BlastConnection, BlastConnectionDto>();
            CreateMap<CreateBlastConnectionRequest, BlastConnection>();
            CreateMap<UpdateBlastConnectionRequest, BlastConnection>();
            CreateMap<BlastConnectionDto, BlastConnection>();

            // SiteBlastingData mappings
            CreateMap<Domain.Entities.BlastingOperations.SiteBlastingData, SiteBlastingDataDto>();
        }
    }
} 
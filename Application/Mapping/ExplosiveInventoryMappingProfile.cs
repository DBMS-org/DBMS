using Application.DTOs.ExplosiveInventory;
using AutoMapper;
using Domain.Entities.ExplosiveInventory;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Explosive Inventory entities
    /// </summary>
    public class ExplosiveInventoryMappingProfile : Profile
    {
        public ExplosiveInventoryMappingProfile()
        {
            // ===== Central Warehouse Inventory =====
            CreateMap<CentralWarehouseInventory, CentralInventoryDto>()
                .ForMember(dest => dest.ExplosiveTypeName, opt => opt.MapFrom(src => src.ExplosiveType.ToString()))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CentralWarehouseName, opt => opt.MapFrom(src => src.CentralWarehouse != null ? src.CentralWarehouse.StoreName : string.Empty))
                .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.AvailableQuantity))
                .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => src.DaysUntilExpiry))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.IsExpiringSoon, opt => opt.MapFrom(src => src.IsExpiringSoon))
                .ForMember(dest => dest.ANFOProperties, opt => opt.MapFrom(src => src.ANFOProperties))
                .ForMember(dest => dest.EmulsionProperties, opt => opt.MapFrom(src => src.EmulsionProperties));

            // ===== ANFO Technical Properties =====
            CreateMap<ANFOTechnicalProperties, ANFOTechnicalPropertiesDto>()
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.ToString()))
                .ForMember(dest => dest.FumeClassName, opt => opt.MapFrom(src => src.FumeClass.ToString()))
                .ForMember(dest => dest.QualityStatusName, opt => opt.MapFrom(src => src.QualityStatus.ToString()))
                .ForMember(dest => dest.IsSafe, opt => opt.MapFrom(src => src.IsSafe()))
                .ForMember(dest => dest.HasAcceptableStorageConditions, opt => opt.MapFrom(src => src.HasAcceptableStorageConditions()));

            // ===== Emulsion Technical Properties =====
            CreateMap<EmulsionTechnicalProperties, EmulsionTechnicalPropertiesDto>()
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.ToString()))
                .ForMember(dest => dest.SensitizationTypeName, opt => opt.MapFrom(src => src.SensitizationType.ToString()))
                .ForMember(dest => dest.FumeClassName, opt => opt.MapFrom(src => src.FumeClass.ToString()))
                .ForMember(dest => dest.QualityStatusName, opt => opt.MapFrom(src => src.QualityStatus.ToString()))
                .ForMember(dest => dest.IsSafe, opt => opt.MapFrom(src => src.IsSafe()))
                .ForMember(dest => dest.HasOptimalStorageConditions, opt => opt.MapFrom(src => src.HasOptimalStorageConditions()))
                .ForMember(dest => dest.IsStable, opt => opt.MapFrom(src => src.IsStable()))
                .ForMember(dest => dest.IsPumpable, opt => opt.MapFrom(src => src.IsPumpable()));

            // ===== Inventory Transfer Request =====
            CreateMap<InventoryTransferRequest, TransferRequestDto>()
                .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.CentralInventory != null ? src.CentralInventory.BatchId : string.Empty))
                .ForMember(dest => dest.ExplosiveTypeName, opt => opt.MapFrom(src => src.CentralInventory != null ? src.CentralInventory.ExplosiveType.ToString() : string.Empty))
                .ForMember(dest => dest.DestinationStoreName, opt => opt.MapFrom(src => src.DestinationStore != null ? src.DestinationStore.StoreName : string.Empty))
                .ForMember(dest => dest.FinalQuantity, opt => opt.MapFrom(src => src.GetFinalQuantity()))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DaysUntilRequired, opt => opt.MapFrom(src => src.GetDaysUntilRequired()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue()))
                .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.IsUrgent()))
                .ForMember(dest => dest.RequestedByUserName, opt => opt.MapFrom(src => src.RequestedByUser != null ? src.RequestedByUser.Name : string.Empty))
                .ForMember(dest => dest.ApprovedByUserName, opt => opt.MapFrom(src => src.ApprovedByUser != null ? src.ApprovedByUser.Name : null))
                .ForMember(dest => dest.ProcessedByUserName, opt => opt.MapFrom(src => src.ProcessedByUser != null ? src.ProcessedByUser.Name : null))
                .ForMember(dest => dest.DispatchedByUserName, opt => opt.MapFrom(src => src.DispatchedByUser != null ? src.DispatchedByUser.Name : null));
        }
    }
}

using AutoMapper;
using Application.DTOs.StoreManagement;
using Domain.Entities.StoreManagement;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Store Management mappings
    /// </summary>
    public class StoreManagementMappingProfile : Profile
    {
        public StoreManagementMappingProfile()
        {
            // Store mappings
            CreateMap<Store, StoreDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Name : null))
                .ForMember(dest => dest.ManagerUserName, opt => opt.MapFrom(src => src.ManagerUser != null ? src.ManagerUser.Name : null))
                .ForMember(dest => dest.ManagerUserEmail, opt => opt.MapFrom(src => src.ManagerUser != null ? src.ManagerUser.Email.Value : null))
                .ForMember(dest => dest.ManagerUserContact, opt => opt.MapFrom(src => src.ManagerUser != null ? src.ManagerUser.OmanPhone : null))
                .ForMember(dest => dest.InventoryItemsCount, opt => opt.MapFrom(src => src.Inventories != null ? src.Inventories.Count : 0))
                .ForMember(dest => dest.UtilizationPercentage, opt => opt.MapFrom(src => src.GetUtilizationRate()));

            CreateMap<CreateStoreRequest, Store>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerUser, opt => opt.Ignore())
                .ForMember(dest => dest.Inventories, opt => opt.Ignore());

            CreateMap<UpdateStoreRequest, Store>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerUser, opt => opt.Ignore())
                .ForMember(dest => dest.Inventories, opt => opt.Ignore());

            // StoreInventory mappings
            CreateMap<StoreInventory, StoreInventoryDto>()
                .ForMember(dest => dest.ExplosiveType, opt => opt.MapFrom(src => src.ExplosiveType.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<CreateStoreInventoryRequest, StoreInventory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Store, opt => opt.Ignore());

            // StoreTransaction mappings
            CreateMap<StoreTransaction, StoreTransactionDto>()
                .ForMember(dest => dest.ExplosiveType, opt => opt.MapFrom(src => src.ExplosiveType.ToString()))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)));

            CreateMap<CreateStoreTransactionRequest, StoreTransaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Store, opt => opt.Ignore())
                .ForMember(dest => dest.StoreInventory, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStore, opt => opt.Ignore())
                .ForMember(dest => dest.ProcessedByUser, opt => opt.Ignore());
        }
    }
}
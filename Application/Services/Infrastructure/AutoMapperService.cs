using Application.Interfaces.Infrastructure;
using AutoMapper;

namespace Application.Services.Infrastructure
{
    /// <summary>
    /// AutoMapper implementation of the mapping service
    /// </summary>
    public class AutoMapperService : IMappingService
    {
        private readonly IMapper _mapper;

        public AutoMapperService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return _mapper.Map<IEnumerable<TDestination>>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }
    }
} 
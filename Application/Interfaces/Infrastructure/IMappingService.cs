namespace Application.Interfaces.Infrastructure
{
    public interface IMappingService
    {
        TDestination Map<TDestination>(object source);

        TDestination Map<TSource, TDestination>(TSource source);

        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
} 
namespace Application.Interfaces.Infrastructure
{
    /// <summary>
    /// Abstraction for object mapping operations
    /// </summary>
    public interface IMappingService
    {
        /// <summary>
        /// Maps an object to the specified destination type
        /// </summary>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Maps from source type to destination type
        /// </summary>
        TDestination Map<TSource, TDestination>(TSource source);

        /// <summary>
        /// Maps a collection of objects to the specified destination type
        /// </summary>
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

        /// <summary>
        /// Maps from source to an existing destination object
        /// </summary>
        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
} 
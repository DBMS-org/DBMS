using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register dispatcher
            services.AddScoped<IDomainEventDispatcher, Services.DomainEventDispatcher>();

            // Register all IDomainEventHandler<> implementations in this assembly
            var handlerInterfaceType = typeof(Services.IDomainEventHandler<>);
            var assembly = Assembly.GetExecutingAssembly();

            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(ti => ti.Interface.IsGenericType && ti.Interface.GetGenericTypeDefinition() == handlerInterfaceType)
                .ToList();

            foreach (var handler in handlerTypes)
            {
                services.AddScoped(handler.Interface, handler.Type);
            }

            return services;
        }
    }
} 
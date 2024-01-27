using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Server.Filters;
using System.Reflection;

namespace FDRWebsite.Server.Abstractions.Filters
{
    public static class FilterRegister
    {
        public static IServiceCollection AddFilters(this IServiceCollection services)
        {
            var interfaces = new Type[] { typeof(IFilter<>) };
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            var implementations = assemblyTypes.Where(
                x => !x.IsInterface &&
                x.GetInterfaces().Any(i => i.IsGenericType && interfaces.Any(type => type.IsAssignableFrom(i.GetGenericTypeDefinition()))));

            foreach (var genericType in implementations)
                services.AddTransient(genericType.GetInterfaces().First(), genericType);

            return services;
        }
    }
}

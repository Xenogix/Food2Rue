using FDRWebsite.Server.Abstractions.Repositories;
using System.Reflection;

namespace FDRWebsite.Server.Repositories
{
    public static class RepositoryRegister
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var interfaces = new Type[] { typeof(IRepositoryBase<,>), typeof(IReadonlyRepositoryBase<,>) };
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

using EducationTech.Repositories.Abstracts;
using EducationTech.Services.Abstract;
using System.Reflection;

namespace EducationTech.Extensions
{
    public static class ServiceColectionExtensions
    {
        public static void InjectRepositpories(this IServiceCollection services)
        {
            var repositoryInterfaceTypes = typeof(IRepository<>).Assembly
                .GetTypes()
                .Where(x => x.IsInterface && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
                .ToList();
            repositoryInterfaceTypes.ForEach(repositoryInterface =>
            {
                var implementation = repositoryInterface.Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.IsClass && repositoryInterface.IsAssignableFrom(x));
                if (implementation != null)
                {
                    services.AddScoped(repositoryInterface, implementation);
                }
            });
        }

        public static void InjectServices(this IServiceCollection services)
        {
            //Get all interface services
            var serviceInterfaceTypes = typeof(IService).Assembly
                .GetTypes()
                .Where(x => x.IsInterface && x.GetInterfaces().Contains(typeof(IService)))
                .ToList();
            serviceInterfaceTypes.ForEach(serviceInterface =>
            {
                var implementation = serviceInterface.Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.IsClass && serviceInterface.IsAssignableFrom(x));
                if (implementation != null)
                {
                    services.AddScoped(serviceInterface, implementation);
                }
            });
        }
    }
}

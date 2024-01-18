using EducationTech.Repositories.Abstracts;
using EducationTech.Seeders;
using EducationTech.Services.Abstract;
using EducationTech.Utilities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EducationTech.Extensions
{
    public static class ServiceColectionExtensions
    {
        public static IServiceCollection InjectRepositpories(this IServiceCollection services)
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

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
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

            return services;

        }

        public static IServiceCollection InjectUtilities(this IServiceCollection services)
        {
            var utilityInterfaceTypes = typeof(IUltils).Assembly
                .GetTypes()
                .Where(x => x.IsInterface && x.GetInterfaces().Contains(typeof(IUltils)))
                .ToList();
            utilityInterfaceTypes.ForEach(utilityInterface =>
            {
                var implementation = utilityInterface.Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.IsClass && utilityInterface.IsAssignableFrom(x));
                if (implementation != null)
                {
                    services.AddScoped(utilityInterface, implementation);
                }
            });

            return services;

        }

        public static IServiceCollection InjectSeeders(this IServiceCollection services)
        {
            var seederTypes = typeof(ISeeder).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .Where(t => t.IsAssignableTo(typeof(ISeeder)))
                    .ToList();

            seederTypes.ForEach(t =>
            {
                if (t.IsAbstract || t.IsInterface)
                {
                    throw new Exception($"Seeder {t.Name} cannot be abstract or interface");
                }
                if (!t.IsAssignableTo(typeof(ISeeder)))
                {
                    throw new Exception($"Seeder {t.Name} must implement ISeeder");
                }
                services.AddScoped(t);
            });

            return services;
        }
    }
}

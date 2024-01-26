using EducationTech.Business.Repositories.Abstract;

namespace EducationTech.Installers
{
    public class Installer_Repositories : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
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

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

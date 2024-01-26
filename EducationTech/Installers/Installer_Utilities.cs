using EducationTech.Utilities.Abstract;

namespace EducationTech.Installers
{
    public class Installer_Utilities : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var utilityInterfaceTypes = typeof(IUtils).Assembly
                .GetTypes()
                .Where(x => x.IsInterface && x.GetInterfaces().Contains(typeof(IUtils)))
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

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

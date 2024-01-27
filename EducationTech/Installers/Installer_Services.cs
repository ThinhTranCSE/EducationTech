using EducationTech.Business.Services.Abstract;
using Serilog;

namespace EducationTech.Installers
{
    public class Installer_Services : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            //Get all interface services
            var serviceInterfaceTypes = typeof(IService).Assembly.ExportedTypes
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

            //Log.Information("Installed Services");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

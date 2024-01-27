using EducationTech.Utilities.Abstract;
using Serilog;

namespace EducationTech.Installers
{
    public class Installer_Utilities : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var utilityInterfaceTypes = typeof(IUtils).Assembly.ExportedTypes
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

            //Log.Information("Installed Utilities");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

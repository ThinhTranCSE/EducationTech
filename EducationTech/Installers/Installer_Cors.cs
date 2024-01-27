using Serilog;

namespace EducationTech.Installers
{
    public class Installer_Cors : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            //Log.Information("Installed Cors");
            return services;

        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

using EducationTech.Databases;

namespace EducationTech.Installers
{
    public class Installer_Database : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MainDatabaseContext>();


            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

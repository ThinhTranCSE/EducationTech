using EducationTech.Storage;

namespace EducationTech.Installers
{
    public class Installer_GlobalUsings : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GlobalReference>();
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

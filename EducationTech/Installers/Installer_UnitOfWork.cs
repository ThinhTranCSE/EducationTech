
using EducationTech.DataAccess.Abstract;

namespace EducationTech.Installers;

public class Installer_UnitOfWork : IInstaller
{
    public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }
}

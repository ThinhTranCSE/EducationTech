namespace EducationTech.Installers;

public class Installer_HttpContextAccessor : IInstaller
{
    public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        return services;
    }

    public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }
}
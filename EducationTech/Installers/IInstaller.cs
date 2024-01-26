namespace EducationTech.Installers
{
    public interface IInstaller
    {
        IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration);
        WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration);
    }
}

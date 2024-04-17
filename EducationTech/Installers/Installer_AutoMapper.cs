using EducationTech.Auth.Policies.Abstract;
using EducationTech.Business.Shared.DTOs.Abstracts;

namespace EducationTech.Installers
{
    public class Installer_AutoMapper : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var dtoTypes = typeof(AbstractDto).Assembly.ExportedTypes
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.IsAssignableTo(typeof(AbstractDto)))
                .ToArray();

            services.AddAutoMapper(dtoTypes);

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

using AutoMapper;
using EducationTech.Auth.Policies.Abstract;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTech.Installers
{
    public class Installer_AutoMapper : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var dtoInstances = typeof(IDto).Assembly.ExportedTypes
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.IsAssignableTo(typeof(IDto)))
                .Select(x => (IDto)Activator.CreateInstance(x))
                .ToList();

            services.AddAutoMapper(config =>
            {
                dtoInstances.ForEach(x => x.Configure(config));
            });

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

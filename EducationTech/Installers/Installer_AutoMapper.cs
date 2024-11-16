using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;

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
                dtoInstances.ForEach(x => x?.Configure(config));
                ConfigNullableProperties(config);
            });

            return services;
        }

        private void ConfigNullableProperties(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<float?, float>().ConvertUsing((src, dest) => src ?? dest);
            cfg.CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
        }
        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

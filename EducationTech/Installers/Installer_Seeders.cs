using EducationTech.Databases.Seeders;

namespace EducationTech.Installers
{
    public class Installer_Seeders : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISeederExecutor, SeederExecutor>();

            var seederTypes = typeof(ISeeder).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .Where(t => t.IsAssignableTo(typeof(ISeeder)))
                    .ToList();

            seederTypes.ForEach(t =>
            {
                if (t.IsAbstract || t.IsInterface)
                {
                    throw new Exception($"Seeder {t.Name} cannot be abstract or interface");
                }
                if (!t.IsAssignableTo(typeof(ISeeder)))
                {
                    throw new Exception($"Seeder {t.Name} must implement ISeeder");
                }
                services.AddScoped(t);
            });

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

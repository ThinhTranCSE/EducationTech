namespace EducationTech.Installers
{
    public static class InstallerExtensions
    {
        public static WebApplicationBuilder InstallServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var installers = typeof(IInstaller).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
            installers.ForEach(installer =>
            {
                installer.InstallServicesToWebApplicationBuilder(builder, configuration);
                installer.InstallServicesToServiceCollection(builder.Services, configuration);
            });

            return builder;
        }

    }
}

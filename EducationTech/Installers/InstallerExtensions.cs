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
            //swap Installer_Logging to be first
            //var loggingInstaller = installers.FirstOrDefault(x => x.GetType().Name == "Installer_Logging");
            //if (loggingInstaller != null)
            //{
            //    installers.Remove(loggingInstaller);
            //    installers.Insert(0, loggingInstaller);
            //}
            installers.ForEach(installer =>
            {
                installer.InstallServicesToWebApplicationBuilder(builder, configuration);
                installer.InstallServicesToServiceCollection(builder.Services, configuration);
            });

            return builder;
        }

    }
}

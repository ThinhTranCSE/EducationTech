using MimeDetective.Storage;
using System.Collections.Immutable;

namespace EducationTech.Installers
{
    public class Installer_MimeDective : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var allDefinitions = new MimeDetective.Definitions.ExhaustiveBuilder()
            {
                UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial
            }.Build();

            var definitions = allDefinitions
                .TrimCategories()
                .TrimDescription()
                .TrimMeta()
                .ToImmutableArray();

            var inspector = new MimeDetective.ContentInspectorBuilder()
            {
                Definitions = definitions
            }.Build();

            services.AddSingleton(inspector);
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}


using EducationTech.RecommendationSystem.Implementations.LoSequenceRecommenders;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.Installers;

public class Installer_Recommendation : IInstaller
{

    public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoSequenceRecommender, LoSequenceRecommender>();

        return services;
    }

    public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }

}

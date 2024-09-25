
using EducationTech.RecommendationSystem.Implementations.LoSequenceRecommenders;
using EducationTech.RecommendationSystem.Implementations.PathGenerations;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.Installers;

public class Installer_Recommendation : IInstaller
{

    public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoSequenceRecommender, LoSequenceRecommender>();
        services.AddScoped<ILoSuitableSelector, LoSuitableSelector>();
        services.AddScoped<ILoPathVisitor, LoPathVisitor>();
        services.AddScoped<ILearningPathRecommender, LearningPathRecommender>();
        return services;
    }

    public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }

}

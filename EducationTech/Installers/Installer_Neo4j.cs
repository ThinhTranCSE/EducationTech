
using EducationTech.Configurations;
using Neo4jClient;

namespace EducationTech.Installers;

public class Installer_Neo4j : IInstaller
{
    public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
    {
        var neo4jConfiguration = configuration.GetSection("Neo4j").Get<Neo4jConfiguration>();
        services.AddSingleton<IGraphClient>(provider =>
        {
            var graphClient = new BoltGraphClient(new Uri(neo4jConfiguration.Uri), neo4jConfiguration.Username, neo4jConfiguration.Password);
            var connectTask = graphClient.ConnectAsync();
            connectTask.Wait();
            return graphClient;
        });

        services.AddSingleton<NeoServerConfiguration>(context =>
        {
            return NeoServerConfiguration.GetConfigurationAsync(new Uri(neo4jConfiguration.Uri), neo4jConfiguration.Username, neo4jConfiguration.Password).GetAwaiter().GetResult();
        });

        services.AddSingleton<IGraphClientFactory, GraphClientFactory>();


        return services;
    }

    public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder;
    }
}



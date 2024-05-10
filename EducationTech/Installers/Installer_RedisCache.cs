using EducationTech.Configurations;
using Serilog;
using StackExchange.Redis;

namespace EducationTech.Installers
{
    public class Installer_RedisCache : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            try
            {

                var redisConfiguration = new RedisConfiguration();
                configuration.GetSection("Redis").Bind(redisConfiguration);
                services.AddSingleton(redisConfiguration);

                if(!redisConfiguration.Enabled) return services;

                services.AddSingleton<IConnectionMultiplexer>(_ =>
                {
                    var host = redisConfiguration.ConnectionString;
                    return ConnectionMultiplexer.Connect(host);
                });

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfiguration.ConnectionString;
                });
            }
            catch
            {
                services.AddDistributedMemoryCache();
            }

            //Log.Information("Installed RedisCache");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

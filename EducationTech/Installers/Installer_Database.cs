using EducationTech.DataAccess.Core;
using Serilog;

namespace EducationTech.Installers
{
    public class Installer_Database : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<EducationTechContext>();

            services.AddScoped<ITransactionManager, TransactionManager>();
            //Log.Information("Installed Database");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

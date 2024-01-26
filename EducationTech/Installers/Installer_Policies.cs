using EducationTech.Auth.Policies.Abstract;

namespace EducationTech.Installers
{
    public class Installer_Policies : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var policies = typeof(IPolicy).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.IsAssignableTo(typeof(IPolicy)))
                .Select(t => (IPolicy)Activator.CreateInstance(t))
                .ToList();

            services.AddAuthorization(options =>
            {
                policies.ForEach(policy =>
                {
                    options.AddPolicy(policy.PolicyName, builder => policy.ApplyRequirements(builder));
                });
            });

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

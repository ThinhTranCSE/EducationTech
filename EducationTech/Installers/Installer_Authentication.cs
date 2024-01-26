using EducationTech.Utilities;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EducationTech.Installers
{
    public class Installer_Authentication : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthUtils, AuthUtils>();
            services
               .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

               })
               .AddJwtBearer(options =>
               {
                   var scope = services.BuildServiceProvider().CreateScope();

                   var authUltils = scope.ServiceProvider.GetRequiredService<IAuthUtils>();
                   options.TokenValidationParameters = new()
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       RequireSignedTokens = true,
                       IssuerSigningKeyResolver = authUltils.KeysResolver,

                       ClockSkew = TimeSpan.Zero
                   };
               });

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

using EducationTech.Business.Business;
using EducationTech.Business.Business.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

namespace EducationTech.Installers
{
    public class Installer_Authentication : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
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

                   var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                   options.TokenValidationParameters = new()
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       RequireSignedTokens = true,
                       IssuerSigningKeyResolver = authService.KeysResolver,

                       ClockSkew = TimeSpan.Zero
                   };
               });

            //Log.Information("Installed Authentication");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

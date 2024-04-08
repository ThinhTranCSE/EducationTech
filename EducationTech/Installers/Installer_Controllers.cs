using EducationTech.Business.Shared.Exceptions.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Net;

namespace EducationTech.Installers
{
    public class Installer_Controllers : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Keys
                                            .SelectMany(key => context.ModelState[key].Errors.Select(x => $"{key}: {x.ErrorMessage}"))
                                            .ToArray();
                    throw new HttpException(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(errors));

                };
            });

            //Log.Information("Installed Controllers");
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

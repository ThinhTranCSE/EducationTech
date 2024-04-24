using EducationTech.Shared.DataStructures;
using EducationTech.Storage;
using Microsoft.AspNetCore.Http.Features;

namespace EducationTech.Installers
{
    public class Installer_UploadLargeFile : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            var globalUsings = new GlobalUsings();
            int chunkSize = globalUsings.UploadChunkSize;
            long sessionTimeOut = globalUsings.UploadSessionTimeOut;


            //UploadFileSessionManager uploadFileSessionManager = new UploadFileSessionManager(chunkSize, sessionTimeOut);
            services.AddSingleton<UploadFileSessionManager>();

            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}

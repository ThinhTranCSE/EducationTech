using EducationTech.Storage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace EducationTech.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureStaticFiles(this WebApplication app)
        {

            //create directory for static files
            var globalUsing = new GlobalUsings();
            string staticFilesPath = Path.Combine(globalUsing.StorageRootPath, "Static");
            if (!Directory.Exists(staticFilesPath))
            {
                Directory.CreateDirectory(staticFilesPath);
            }

            //var provider = new FileExtensionContentTypeProvider();
            //// Add new mappings
            //provider.Mappings[".m3u8"] = "application/x-mpegURL";
            //provider.Mappings[".ts"] = "video/MP2T";




            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    ContentTypeProvider = provider,
            //    FileProvider = new PhysicalFileProvider(staticFilesPath),
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
            //        //file place in Public folder do not need to authenticate
            //        string[] strings = ctx.Context.Request.Path.Value.Split("/");
            //        if (strings.Length > 1 && strings[1] == "Public")
            //        {
            //            return;
            //        }
            //        using (var scope = app.Services.CreateScope())
            //        {
            //            var authUtils = scope.ServiceProvider.GetRequiredService<IAuthUtils>();
            //            User? user = authUtils.GetUserFromToken(ctx.Context.Request.Headers.Authorization);

            //            if (user == null)
            //            {
            //                throw new HttpException(HttpStatusCode.Unauthorized, "Unauthorized");
            //            }
            //        }

            //    }
            //});



            return app;
        }
    }
}

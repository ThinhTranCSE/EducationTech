using EducationTech.DataAccess.Seeders;
using EducationTech.Extensions;
using EducationTech.Installers;
using EducationTech.Middlewares;
using EducationTech.Shared.Utilities;
using EducationTech.Storage;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace EducationTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();

            builder.InstallServices(builder.Configuration);

            HandleSeederCommand(builder, args);

            var app = builder.Build();

            app.ResolveAllConsumers();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors(buidler =>
            {
                buidler.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseSerilogRequestLogging();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<HttpExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureStaticFiles();

            app.UseMiddleware<ResponseRestructureMiddleware>();

            app.MapControllers();

            app.Run();
        }

        private static void HandleSeederCommand(WebApplicationBuilder builder, string[] args)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {

                CancellationToken cancellationToken = cancellationTokenSource.Token;
                var providers = builder.Services.BuildServiceProvider();
                using (var scope = providers.CreateScope())
                {
                    ISeederExecutor seederExecutor = scope.ServiceProvider.GetRequiredService<ISeederExecutor>();
                    seederExecutor.RegisterSeeders(scope);

                    seederExecutor.Execute(cancellationTokenSource, args);
                    //seederExecutor.Execute(cancellationTokenSource, new string[] {"seeder", "CourseSeeder"});
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private static async Task TestConvertVideo(WebApplication app)
        {
            var converter = new VideoConverter();
            var globalUsings = app.Services.GetRequiredService<GlobalUsings>();
            var contentRootPath = globalUsings.StorageRootPath; 
            await converter.From(Path.Combine(contentRootPath, "Static", "video.mp4"))
                .To(Path.Combine(contentRootPath, "Static", "ClipTest"))
                .ProcessAsync();
        }
    }
}
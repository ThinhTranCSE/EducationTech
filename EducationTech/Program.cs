using EducationTech.DataAccess.Seeders;
using EducationTech.Extensions;
using EducationTech.Installers;
using EducationTech.Middlewares;
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

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = cancellationTokenSource.Token;
                HandleSeederCommand(builder, args, cancellationTokenSource);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.ResolveAllConsumers();

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

        private static void HandleSeederCommand(WebApplicationBuilder builder, string[] args, CancellationTokenSource cancellationTokenSource)
        {
            var providers = builder.Services.BuildServiceProvider();
            using var scope = providers.CreateScope();
            var seederExecutor = scope.ServiceProvider.GetRequiredService<ISeederExecutor>();
            seederExecutor.RegisterSeeders(scope);
            seederExecutor.Execute(cancellationTokenSource, args);
        }
    }
}
using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Formatting.Compact;

namespace EducationTech.Installers
{
    public class Installer_Logging : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog();

            CreateFolder("Logs");

            var loggerConfiguration = new LoggerConfiguration();


            loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .WriteTo.Async(a =>
                {
                    a.Logger(lc =>
                    {
                        lc.WriteTo.Console(
                            theme: AnsiConsoleTheme.Literate,
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"

                            )
                        .MinimumLevel.Debug();

                    });

                    a.Logger(lc =>
                    {
                        lc.WriteTo.Logger(lc =>
                        {
                            lc.MinimumLevel.Information()
                              .WriteTo.File(
                                new RenderedCompactJsonFormatter(),
                                "Logs/log-.txt",
                                rollingInterval: RollingInterval.Day,
                                fileSizeLimitBytes: 10000000,
                                rollOnFileSizeLimit: true
                                );
                        })
                        .MinimumLevel.Information();

                    });

                    a.Logger(lc =>
                    {
                        lc.MinimumLevel.Information()
                            .WriteTo.Seq("http://localhost:5341", LogEventLevel.Information);
                    });

                });


            var logger = loggerConfiguration.CreateLogger();

            Log.Logger = logger;
            builder.Logging.AddSerilog(logger);

            builder.Services.AddSingleton<Serilog.ILogger>(logger);
            return builder;
        }

        private static void CreateFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }
    }
}

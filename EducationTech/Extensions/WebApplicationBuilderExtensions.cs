using EducationTech.Services.Abstract;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using System.Runtime.Serialization;

namespace EducationTech.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
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
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .WriteTo.Async(a =>
                {
                    a.Logger(lc =>
                    {
                        lc.MinimumLevel.Debug()
                            .WriteTo.Console(
                              theme: AnsiConsoleTheme.Literate,
                              outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                            );
                    });

                    a.Logger(lc =>
                    {
                        lc.MinimumLevel.Information()
                            .WriteTo.Logger(lc =>
                            {
                                lc.MinimumLevel.Information()
                                  .WriteTo.File(
                                    new RenderedCompactJsonFormatter(),
                                    "Logs/log-.txt",
                                    rollingInterval: RollingInterval.Day,
                                    fileSizeLimitBytes: 10000000,
                                    rollOnFileSizeLimit: true
                                    ) ;
                             });
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

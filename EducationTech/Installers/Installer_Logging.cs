﻿using EducationTech.DataAccess.Shared.Enums;
using EducationTech.Storage;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

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

            string storagePath = GlobalReference.Instance.StorageRootPath;
            CreateFolder(Path.Combine(storagePath, "Logs"));

            var loggerConfiguration = new LoggerConfiguration();


            loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(a =>
                {
                    a.Logger(lc =>
                    {
                        lc
                        .WriteTo.Console(
                                theme: AnsiConsoleTheme.Literate,
                                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                                )
                        .Filter.ByIncludingOnly(Matching.WithProperty("LoggingContext", LoggingContext.Normal.ToString()))
                        .MinimumLevel.Debug();

                    });

                    a.Logger(lc =>
                    {
                        CreateFolder(Path.Combine(storagePath, "Logs"));
                        lc
                        .WriteTo.File(
                                new RenderedCompactJsonFormatter(),
                                Path.Combine(storagePath, "Logs", "log-.txt"),
                                rollingInterval: RollingInterval.Day,
                                fileSizeLimitBytes: 10 * 1024 * 1024,
                                rollOnFileSizeLimit: true
                                )
                        .Filter.ByIncludingOnly(Matching.WithProperty("LoggingContext", LoggingContext.Normal.ToString()))
                        .MinimumLevel.Information();
                    });

                    a.Logger(lc =>
                    {
                        lc
                        .WriteTo.Seq("http://localhost:5341", LogEventLevel.Information)
                        .Filter.ByIncludingOnly(Matching.WithProperty("LoggingContext", LoggingContext.Normal.ToString()))
                        .MinimumLevel.Information();
                    });

                    a.Logger(lc =>
                    {
                        CreateFolder(Path.Combine(storagePath, "Logs/Database/SlowQueries"));
                        lc
                        .WriteTo.File(
                            new RenderedCompactJsonFormatter(),
                            Path.Combine(storagePath, "Logs", "Database", "SlowQueries", "slow-queries-.txt"),
                            rollingInterval: RollingInterval.Day,
                            fileSizeLimitBytes: 10 * 1024 * 1024
                            )
                        .Filter.ByIncludingOnly(Matching.WithProperty("LoggingContext", LoggingContext.SlowQueries.ToString()))
                        .MinimumLevel.Debug();
                    });

                    a.Logger(lc =>
                    {
                        CreateFolder(Path.Combine(storagePath, "Logs/Database"));
                        lc
                        .WriteTo.File(
                            new RenderedCompactJsonFormatter(),
                             Path.Combine(storagePath, "Logs", "Database", "queries-.txt"),
                            rollingInterval: RollingInterval.Day,
                            fileSizeLimitBytes: 10 * 1024 * 1024
                            )
                        .Filter.ByIncludingOnly(Matching.WithProperty("LoggingContext", LoggingContext.Queries.ToString()))
                        .MinimumLevel.Debug();
                    });
                });


            var logger = loggerConfiguration.CreateLogger();
            var normalLogger = logger.ForContext("LoggingContext", LoggingContext.Normal.ToString());
            Log.Logger = normalLogger;
            builder.Logging.AddSerilog(normalLogger);

            builder.Services.AddSingleton<Serilog.ILogger>(normalLogger);

            //Log.Information("Installed Logging");
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

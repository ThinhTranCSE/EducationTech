using EducationTech.Business.Services.Business.Interfaces;
using Serilog;
using Serilog.Events;

namespace EducationTech.Business.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly Serilog.ILogger _logger;

        public LoggingService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Verbose(string message)
        {
            Log(LogEventLevel.Verbose, message);
        }
        public void Debug(string message)
        {
            Log(LogEventLevel.Debug, message);
        }

        public void Information(string message)
        {
            Log(LogEventLevel.Information, message);
        }

        public void Warning(string message)
        {
            Log(LogEventLevel.Warning, message);
        }

        public void Error(string message)
        {
            Log(LogEventLevel.Error, message);
        }

        public void Fatal(string message)
        {
            Log(LogEventLevel.Fatal, message);
        }

        public void Log(LogEventLevel level, string message)
        {
            _logger.Write(level, message);
        }

        public void Log(LogEventLevel level, string messageTemplate, params object?[]? parameters)
        {
            _logger.Write(level, messageTemplate, parameters);
        }
    }
}

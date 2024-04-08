using EducationTech.Business.Abstract;
using Serilog.Events;

namespace EducationTech.Business.Business.Interfaces
{
    public interface ILoggingService : IService
    {
        void Verbose(string message);
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
        void Log(LogEventLevel level, string message);
        void Log(LogEventLevel level, string messageTemplate, params object?[]? parameters);
    }
}

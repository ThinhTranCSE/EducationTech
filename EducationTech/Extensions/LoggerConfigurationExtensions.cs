using EducationTech.DataAccess.Shared.Enums;
using EducationTech.Enums;
using Serilog;

namespace EducationTech.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration AsNormalContext(this LoggerConfiguration config)
        {
            config.Filter.ByExcluding(e =>
            {
                return e.Properties.ContainsKey("LoggingContext")
                    && Enum.GetNames<LoggingContext>()
                        .Where(n => n != LoggingContext.Normal.ToString())
                        .Contains(e.Properties["LoggingContext"].ToString());
            });

            return config;
        }
    }
}

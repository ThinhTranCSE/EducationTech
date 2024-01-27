using EducationTech.Enums;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Serilog.Context;
using System.Data.Common;

namespace EducationTech.Databases.Interceptors
{
    public class LogSlowQueriesInterceptor : DbCommandInterceptor
    {
        private readonly int _thresholdMilliseconds;
        private readonly Serilog.ILogger _logger;

        public LogSlowQueriesInterceptor(int thresholdMilliseconds)
        {
            _thresholdMilliseconds = thresholdMilliseconds;

            _logger = Log.ForContext("LoggingContext", LoggingContext.SlowQueries.ToString());
            
        }

        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            if(eventData.Duration.TotalMilliseconds > _thresholdMilliseconds)
            {
                _logger.Warning($"Slow query: {command.CommandText} took {eventData.Duration.TotalMilliseconds}ms");
            }


            return base.ReaderExecuted(command, eventData, result);
        }

        public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds > _thresholdMilliseconds)
            {
                _logger.Warning($"Slow query: {command.CommandText} took {eventData.Duration.TotalMilliseconds}ms");
            }

            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);

        }
    }
}

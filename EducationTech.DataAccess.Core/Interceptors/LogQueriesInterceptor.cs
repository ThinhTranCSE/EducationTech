using EducationTech.DataAccess.Shared.Enums;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System.Data.Common;

namespace EducationTech.DataAccess.Core.Interceptors
{
    public class LogQueriesInterceptor : DbCommandInterceptor
    {
        private readonly ILogger _logger;

        public LogQueriesInterceptor()
        {
            _logger = Log.ForContext("LoggingContext", LoggingContext.Queries.ToString());
        }

        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            _logger.Debug($"Query: {command.CommandText} took {eventData.Duration.TotalMilliseconds}ms");

            return base.ReaderExecuted(command, eventData, result);
        }

        public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            _logger.Debug($"Query: {command.CommandText} took {eventData.Duration.TotalMilliseconds}ms");

            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);

        }
    }
}

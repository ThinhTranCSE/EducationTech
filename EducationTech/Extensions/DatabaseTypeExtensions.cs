using EducationTech.Enums;

namespace EducationTech.Extensions
{
    public static class DatabaseTypeExtensions
    {
        public static string ToConnectionString(this DatabaseType databaseType)
        {
            string connectionString = databaseType switch
            {
                DatabaseType.MySql => Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"),
                //DatabaseType.SqlServer => Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING"),
                //DatabaseType.PostgreSql => Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING"),
                //DatabaseType.Sqlite => Environment.GetEnvironmentVariable("SQLITE_CONNECTION_STRING"),
                _ => throw new ArgumentOutOfRangeException()
            };

            return connectionString;
        }
    }
}

using EducationTech.Databases.Factories;
using EducationTech.Databases.Providers.MySql;
using EducationTech.Enums;

namespace EducationTech.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var databaseType = configuration.GetSection("Database").GetSection("DatabaseType").Get<DatabaseType>();
            switch (databaseType)
            {
                case DatabaseType.MySql:
                    DbContextFactory.Instance.UseMySQL(builder);
                    break;
                case DatabaseType.SqlServer:
                    break;
                case DatabaseType.PostgreSql:
                    break;
                case DatabaseType.Sqlite:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(databaseType.ToString());
            }
        }
    }
}

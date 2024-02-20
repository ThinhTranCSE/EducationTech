using EducationTech.Databases.Factories;
using EducationTech.Databases.Providers.MySql;
using EducationTech.Enums;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder RegisterDbContext(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var databaseType = configuration.GetSection("Database").GetSection("DatabaseType").Get<DatabaseType>();
            switch (databaseType)
            {
                case DatabaseType.MySql:
                    DbContextFactory.Instance.UseMySQL(optionsBuilder, configuration);
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

            return optionsBuilder;
        }
    }
}

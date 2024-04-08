using EducationTech.DataAccess.Core.Factories;
using EducationTech.DataAccess.Core.Providers.MySql;
using EducationTech.DataAccess.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EducationTech.DataAccess.Core.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder RegisterDbContext(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var databaseType = configuration.GetSection("Database").GetSection("DatabaseType").Value.ToDatabaseType();

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

using EducationTech.DataAccess.Core.Extensions;
using EducationTech.DataAccess.Core.Factories;
using EducationTech.DataAccess.Core.Providers.MySql;
using EducationTech.DataAccess.Shared.Enums;
using EducationTech.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EducationTechContext>
    {
        public EducationTechContext CreateDbContext(string[] args)
        {
            string appjsonPath = Path.Combine(new GlobalUsings().ContentRootPath, "appsettings.json");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appjsonPath)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EducationTechContext>();
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
            return new EducationTechContext(optionsBuilder.Options, configuration);
        }
    }


}

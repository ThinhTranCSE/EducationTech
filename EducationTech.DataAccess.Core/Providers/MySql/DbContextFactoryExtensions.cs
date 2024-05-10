using EducationTech.DataAccess.Core.Factories;
using EducationTech.DataAccess.Core.Providers.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EducationTech.DataAccess.Core.Providers.MySql
{
    public static class DbContextFactoryExtensions
    {
        public static void UseMySQL(this DbContextFactory factory, DbContextOptionsBuilder optionsBuidler, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("Database").GetSection("ConnectionStrings")
                .GetSection("MySql").Value;
            optionsBuidler
                //.UseLazyLoadingProxies(false)
                .UseMySQL(connectionString);
        }
    }
}

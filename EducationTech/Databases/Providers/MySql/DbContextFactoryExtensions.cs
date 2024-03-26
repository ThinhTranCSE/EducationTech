using EducationTech.Databases.Factories;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases.Providers.MySql
{
    public static class DbContextFactoryExtensions
    {
        public static void UseMySQL(this DbContextFactory factory, DbContextOptionsBuilder optionsBuidler, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("Database").GetSection("ConnectionStrings").GetValue<string>("MySql");
            optionsBuidler
                .UseLazyLoadingProxies(false)
                .UseMySQL(connectionString);
        }
    }
}

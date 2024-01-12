using EducationTech.Databases.Factories;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases.Providers.MySql
{
    public static class DbContextFactoryExtensions
    {
        public static void UseMySQL(this DbContextFactory factory, WebApplicationBuilder webApplicationBuilder)
        {
            string connectionString = webApplicationBuilder.Configuration.GetSection("Database").GetSection("ConnectionStrings").GetValue<string>("MySql");
            webApplicationBuilder.Services.AddDbContext<MainDatabaseContext>(options =>
            {
                options.UseMySQL(connectionString);
            });
        }
    }
}

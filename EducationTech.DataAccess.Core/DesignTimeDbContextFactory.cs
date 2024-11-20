using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EducationTech.DataAccess.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EducationTechContext>
    {
        public EducationTechContext CreateDbContext(string[] args)
        {
            string appjsonPath = Path.Combine("appsettings.json");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appjsonPath)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EducationTechContext>();

            optionsBuilder.RegisterDbContext(configuration);

            return new EducationTechContext(optionsBuilder.Options, configuration);
        }
    }


}

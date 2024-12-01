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
            string appsettingsPath = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "EducationTech",
                "appsettings.json");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(appsettingsPath))
                .AddJsonFile(appsettingsPath)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EducationTechContext>();

            optionsBuilder.RegisterDbContext(configuration);

            return new EducationTechContext(optionsBuilder.Options, configuration);
        }
    }


}

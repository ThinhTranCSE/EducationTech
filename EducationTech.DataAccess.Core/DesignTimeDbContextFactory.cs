using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Core.Extensions;
using EducationTech.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EducationTech.DataAccess.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EducationTechContext>
    {
        public EducationTechContext CreateDbContext(string[] args)
        {
            string appjsonPath = Path.Combine(GlobalReference.Instance.ContentRootPath, "appsettings.json");
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

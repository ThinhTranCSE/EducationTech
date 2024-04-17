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
            
            optionsBuilder.RegisterDbContext(configuration);

            return new EducationTechContext(optionsBuilder.Options, configuration);
        }
    }


}

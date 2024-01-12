using EducationTech.Databases.Providers.MySql;
using EducationTech.Enums;
using EducationTech.Extensions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;

namespace EducationTech.Databases.Factories
{
    public class DbContextFactory
    {
        private static DbContextFactory _instance { get; set; }
        public static DbContextFactory Instance => GetInstance();
        private DbContextFactory()
        {
        }

        public static DbContextFactory GetInstance()
        {
            _instance ??= new DbContextFactory();
            return _instance;
        }
    }
}



namespace EducationTech.DataAccess.Core.Factories
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

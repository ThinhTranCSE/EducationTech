using EducationTech.DataAccess.Core;

namespace TestFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new DesignTimeDbContextFactory();
            var context = factory.CreateDbContext(args);
        }
    }
}
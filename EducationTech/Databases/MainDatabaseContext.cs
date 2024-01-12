using EducationTech.Databases.Factories;
using EducationTech.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases
{
    public class MainDatabaseContext : DbContext
    {
        public MainDatabaseContext(DbContextOptions<MainDatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}

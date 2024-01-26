using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases.Seeders
{
    public abstract class Seeder : ISeeder
    {
        protected readonly MainDatabaseContext _context;

        protected Seeder(MainDatabaseContext context)
        {
            _context = context;
        }

        public abstract void Seed();
    }
}

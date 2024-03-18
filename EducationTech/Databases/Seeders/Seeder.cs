using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases.Seeders
{
    public abstract class Seeder : ISeeder
    {
        protected readonly EducationTechContext _context;

        protected Seeder(EducationTechContext context)
        {
            _context = context;
        }

        public abstract void Seed();
    }
}

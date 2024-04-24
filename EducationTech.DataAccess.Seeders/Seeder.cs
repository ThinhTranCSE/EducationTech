using EducationTech.DataAccess.Core;

namespace EducationTech.DataAccess.Seeders
{
    public abstract class Seeder : ISeeder
    {
        public virtual int Piority => 0;
        protected readonly EducationTechContext _context;

        protected Seeder(EducationTechContext context)
        {
            _context = context;
        }

        public abstract void Seed();
    }
}

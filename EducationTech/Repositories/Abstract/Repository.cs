using EducationTech.Databases;

namespace EducationTech.Repositories.Abstract
{
    public abstract class Repository
    {
        protected readonly MainDatabaseContext _context;
        protected Repository(MainDatabaseContext context) { _context = context; }
    }
}

using EducationTech.Databases;

namespace EducationTech.Business.Repositories.Abstract
{
    public abstract class Repository
    {
        protected readonly MainDatabaseContext _context;
        protected Repository(MainDatabaseContext context) { _context = context; }
    }
}

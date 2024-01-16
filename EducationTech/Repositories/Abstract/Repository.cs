using EducationTech.Databases;
using EducationTech.Repositories.Abstracts;

namespace EducationTech.Repositories.Abstract
{
    public abstract class Repository
    {
        protected readonly MainDatabaseContext _context;
        protected Repository(MainDatabaseContext context) { _context = context; }
    }
}

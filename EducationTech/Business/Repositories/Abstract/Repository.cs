using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.Business.Repositories.Abstract
{
    public abstract class Repository
    {
        protected readonly MainDatabaseContext _context;

        protected Repository(MainDatabaseContext context) { _context = context; }

    }
}

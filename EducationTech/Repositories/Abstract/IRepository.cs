using Microsoft.EntityFrameworkCore;

namespace EducationTech.Repositories.Abstracts
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> model { get; }

    }
}

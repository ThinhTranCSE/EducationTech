using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Model { get; }

    }
}

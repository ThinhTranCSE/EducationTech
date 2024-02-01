using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Abstract
{
    public interface IRepository<T>
        where T : class
    {
        DbSet<T> Model { get; }

        Task<IEnumerable<T>> Get();

        Task<T?> Insert(T insertObject);

        Task<IEnumerable<T>> Insert(IEnumerable<T> insertObjects);

        Task<T?> Update(T updateObject);

        Task<T?> Delete(T deleteObject);
    }
}

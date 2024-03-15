using EducationTech.Business.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationTech.Business.Repositories.Abstract
{
    public interface IRepository<T>
        where T : Model
    {
        DbSet<T> Model { get; }

        Task<IQueryable<T>> GetMany(Expression<Func<T, bool>>? filter = null, bool tracked = true, bool executed = false);
        Task<T?> Get(Expression<Func<T, bool>> filter, bool tracked = true);

        Task<IQueryable<T>> Insert(IEnumerable<T> entities);
        Task<T?> Insert(T entity);

        Task<T?> Update(T entity);

        Task<T?> Delete(T entity);
    }
}

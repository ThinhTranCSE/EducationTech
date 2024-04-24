using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EducationTech.DataAccess.Abstract
{
    public interface IRepository<T>
        where T : Entity
    {
        DbSet<T> Model { get; }
        EntityEntry<T> Entry(T entity);
        Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, bool executed = false);
        Task<T?> GetSingle(Expression<Func<T, bool>> filter, bool tracked = true);

        Task<IQueryable<T>> Insert(IEnumerable<T> entities, bool executed = false);
        Task<T?> Insert(T entity, bool executed = false);

        Task<T?> Update(T entity, bool executed = false);

        Task<T?> Delete(T entity, bool executed = false);

        Task<IQueryable<T>> Delete(IEnumerable<T> entities, bool executed = false);
    }
}

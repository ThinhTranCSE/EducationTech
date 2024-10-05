using EducationTech.DataAccess.Entities.Abstract;
using System.Linq.Expressions;

namespace EducationTech.DataAccess.Abstract
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        TEntity? GetById<TId>(TId id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}

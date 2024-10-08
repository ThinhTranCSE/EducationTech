using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Abstract;
using System.Linq.Expressions;

namespace EducationTech.DataAccess.Abstract;

public abstract class Repository<T> : IRepository<T> where T : Entity
{
    public Repository(IMainDatabaseContext context)
    {
        _context = context;
    }
    protected IMainDatabaseContext _context { get; set; }


    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Instance.Set<T>().Where(expression);
    }
    public IQueryable<T> GetAll()
    {
        return _context.Instance.Set<T>().AsQueryable();
    }

    public T? GetById<TId>(TId id)
    {
        return _context.Instance.Set<T>().Find(id);
    }
    public void Add(T entity)
    {
        _context.Instance.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _context.Instance.Set<T>().AddRange(entities);
    }

    public void Remove(T entity)
    {
        _context.Instance.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Instance.Set<T>().RemoveRange(entities);
    }
}

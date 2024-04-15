using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EducationTech.DataAccess.Abstract
{
    public abstract class Repository<T> : IRepository<T>
        where T : Model
    {
        protected readonly EducationTechContext _context;

        protected Repository(EducationTechContext context) { _context = context; }

        public abstract DbSet<T> Model { get; }
        public EntityEntry<T> Entry(T entity)
        {
            _context.Database.BeginTransaction();
            return _context.Entry(entity);

        }

        public virtual async Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, bool executed = false)
        {
            try
            {
                IQueryable<T> query = Model;
                if (!tracked)
                {
                    query = query.AsNoTracking();
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return executed ? query.ToList().AsQueryable() : query;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<T?> GetSingle(Expression<Func<T, bool>> filter, bool tracked = true)
        {
            try
            {
                IQueryable<T> query = Model;
                if (!tracked)
                {
                    query = query.AsNoTracking();
                }
                query = query.Where(filter);

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public virtual async Task<IQueryable<T>> Insert(IEnumerable<T> entities, bool executed = false)
        {
            try
            {
                await Model.AddRangeAsync(entities);
                if (executed)
                {
                    await _context.SaveChangesAsync();
                }
                return entities.AsQueryable();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<T?> Insert(T entity, bool executed = false)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Added;
                if (executed)
                {
                    await _context.SaveChangesAsync();
                }
                return entity;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<T?> Update(T entity, bool executed = false)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                if (executed)
                {
                    await _context.SaveChangesAsync();
                }
                return entity;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public virtual async Task<T?> Delete(T entity, bool executed = false)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Deleted;
                if (executed)
                {
                    await _context.SaveChangesAsync();
                }
                return entity;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<IQueryable<T>> Delete(IEnumerable<T> entities, bool executed = false)
        {
            try
            {
                _context.RemoveRange(entities);
                if (executed)
                {
                    await _context.SaveChangesAsync();
                }
                return entities.AsQueryable();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }


}

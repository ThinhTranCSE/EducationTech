using EducationTech.Business.Models.Abstract;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EducationTech.Business.Repositories.Abstract
{
    public abstract class Repository<T> : IRepository<T>
        where T : Model
    {
        protected readonly MainDatabaseContext _context;

        protected Repository(MainDatabaseContext context) { _context = context; }

        public abstract DbSet<T> Model { get; }

        public virtual async Task<IQueryable<T>> GetMany(Expression<Func<T, bool>>? filter = null, bool tracked = true, bool executed = false)
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

        public virtual async Task<T?> Get(Expression<Func<T, bool>> filter , bool tracked = true)
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


        public virtual async Task<IQueryable<T>> Insert(IEnumerable<T> entities)
        {
            try
            {
                await Model.AddRangeAsync(entities);
                await _context.SaveChangesAsync();  
                return entities.AsQueryable();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<T?> Insert(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual async Task<T?> Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public virtual async Task<T?> Delete(T entity)
        {
            if (entity.SoftDelete)
            {
                entity.DeletedAt = DateTimeOffset.Now;
                return await Update(entity);
            }
            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

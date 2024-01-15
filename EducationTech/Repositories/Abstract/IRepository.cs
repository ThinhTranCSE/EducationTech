using Microsoft.EntityFrameworkCore;

namespace EducationTech.Repositories.Abstracts
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Query { get; }
        Task<T?> Get(int id);
        Task<T[]> Get<TGetDto>(TGetDto model);
        Task<T?> Insert<TInsertDto>(TInsertDto model);
        Task<T?> Update<TUpdateDto>(int id, TUpdateDto model);
        Task<T?> Delete(int id);
    }
}

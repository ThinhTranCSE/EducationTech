using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Business.Interfaces
{
    public interface ICacheService : IService
    {
        Task<string?> GetAsync(string key);
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object? value, TimeSpan timeSpan);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);

        Task<T?> TryGetAndSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan timeSpan); 
    }
}

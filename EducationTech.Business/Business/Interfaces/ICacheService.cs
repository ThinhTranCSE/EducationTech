using EducationTech.Business.Abstract;

namespace EducationTech.Business.Business.Interfaces
{
    public interface ICacheService : IService
    {
        Task<string?> GetAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync(string key, object? value, TimeSpan timeSpan, CancellationToken token = default);
        Task RemoveAsync(string key, CancellationToken token = default);
        Task RemoveByPatternAsync(string pattern, CancellationToken token = default);

        Task<T?> TryGetAndSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan timeSpan, CancellationToken token = default);
    }
}

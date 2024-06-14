using System;
namespace Demo.Maui.Repositories.Interfaces;

public interface ICacheRepository
{
    public Task<T> GetCacheEntityAsync<T>(string key, bool useStaticCache = false, bool neverExpire = false);
    public Task<T> UpdateCacheEntityAsync<T>(string key, T entity);
    public Task<IEnumerable<T>> UpdateCacheEntityAsync<T>(string key, IEnumerable<T> entities);
    public Task<IEnumerable<T>> RemoveCacheEntityAsync<T>(string key, T entity);
    public Task<IEnumerable<T>> AddCacheEntityAsync<T>(string key, T entity);
    public Task ClearCacheEntitiesAsync();
    public Task ClearCacheEntityAsync<T>(string key);

}


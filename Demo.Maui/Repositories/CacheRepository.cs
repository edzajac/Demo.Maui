using Demo.Maui.Constants;
using Demo.Maui.Repositories.Interfaces;
using Demo.Maui.Services.Interfaces;

namespace Demo.Maui.Repositories;

public class CacheRepository : ICacheRepository
{
    #region Fields

    private readonly ICacheService _cacheService;

    #endregion

    #region Constructor

    public CacheRepository(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get cached entity by type,
    /// if useStaticCache, then always return cache value if under validation time
    /// if neverExpire, then always return cache
    /// else update and get latest
    /// </summary>
    /// <param name="key"></param>
    /// <param name="useStaticCache"></param>
    /// <param name="neverExpire"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T> GetCacheEntityAsync<T>(string key, bool useStaticCache = false, bool neverExpire = false)
        => GetCachedEntityAsync<T>(key, useStaticCache, neverExpire);

    /// <summary>
    /// Update cached entity and get latest cached value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> UpdateCacheEntityAsync<T>(string key, T entity)
    {
        await SetCachedEntityAsync(key, entity);
        return await GetCachedEntityAsync<T>(key);
    }

    /// <summary>
    /// Update cached entities list and get latest cached values
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entities"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<T>> UpdateCacheEntityAsync<T>(string key, IEnumerable<T> entities)
    {
        await SetCachedEntityAsync(key, entities);
        return await GetCachedEntityAsync<IEnumerable<T>>(key);
    }

    /// <summary>
    /// Remove cached entity and get latest cached value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<T>> RemoveCacheEntityAsync<T>(string key, T entity)
    {
        var entities = await GetCacheEntityAsync<IEnumerable<T>>(key, neverExpire: true) ?? new List<T>();
        var list = entities.ToList();
        list.Remove(entity);

        return await UpdateCacheEntityAsync<T>(key, list);
    }

    /// <summary>
    /// Add cached entity and get latest cached value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<T>> AddCacheEntityAsync<T>(string key, T entity)
    {
        var entities = await GetCacheEntityAsync<IEnumerable<T>>(key, neverExpire: true) ?? new List<T>();
        var list = entities.ToList();
        list.Add(entity);

        return await UpdateCacheEntityAsync<T>(key, list);
    }

    /// <summary>
    /// Clear all cached entities
    /// </summary>
    public async Task ClearCacheEntitiesAsync()
        => await _cacheService.ClearAsync();

    /// <summary>
    /// Remove cached entity
    /// </summary>
    public async Task ClearCacheEntityAsync<T>(string key)
        => await _cacheService.RemoveAsync<T>(key);


    private Task SetCachedEntityAsync<T>(string key, IEnumerable<T> entities)
        => _cacheService.SetAsync(key, entities);

    private Task SetCachedEntityAsync<T>(string key, T entity)
        => _cacheService.SetAsync(key, entity);

    private async Task<T> GetCachedEntityAsync<T>(string key, bool useStaticCache = false, bool neverExpire = false)
    {
        var cache = await _cacheService.GetCreatedAtAsync<T>(key);

        //if new then return null
        if (cache == null) return default;

        //if always cached
        if (neverExpire)
            return await _cacheService.GetAsync<T>(key);

        var dateTimeDifference = DateTime.UtcNow.Subtract(cache.Value.DateTime).Minutes;
        if (dateTimeDifference <=
            //if over time in minutes, invalidate and return null
            (useStaticCache ? CacheConstants.Cache.StaticInvalidationTime : CacheConstants.Cache.DefaultInvalidationTime))
            return await _cacheService.GetAsync<T>(key);

        //else update and return cache
        //clear to get latest api data
        await _cacheService.RemoveAsync<T>(key);
        return default;
    }

    #endregion
}


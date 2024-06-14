namespace Demo.Maui.Services.Interfaces;

public interface ICacheService
{
    public Task<DateTimeOffset?> GetCreatedAtAsync<T>(string key, CancellationToken cancellationToken = default);
    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>?> GetAllAsync<T>(CancellationToken cancellationToken = default);
    public Task<bool> RemoveAsync<T>(string key, CancellationToken cancellationToken = default);
    public Task<bool> ClearAsync(CancellationToken cancellationToken = default);
}


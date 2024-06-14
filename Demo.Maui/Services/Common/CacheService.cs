using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Akavache;
using Demo.Maui.Constants;
using Demo.Maui.Services.Interfaces;

namespace Demo.Maui.Services.Common;

public class CacheService : ICacheService
{
    #region Constructor

    public CacheService()
	{
        Registrations.Start(CacheConstants.Cache.Name);
        BlobCache.ApplicationName = CacheConstants.Cache.Name;
        BlobCache.Secure.Vacuum();
    }

    #endregion

    #region Methods

    public Task<DateTimeOffset?> GetCreatedAtAsync<T>(string key, CancellationToken cancellationToken = default)
            => BlobCache.Secure.GetObjectCreatedAt<T>(key).ToTask(cancellationToken);

    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        => BlobCache.Secure.InsertObject(key, value).ToTask(cancellationToken);

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => BlobCache.Secure.GetObject<T>(key).Catch(Observable.Return<T?>(default)).ToTask(cancellationToken);

    public Task<IEnumerable<T>?> GetAllAsync<T>(CancellationToken cancellationToken = default)
        => BlobCache.Secure.GetAllObjects<T>().Catch(Observable.Return<IEnumerable<T>?>(default)).ToTask(cancellationToken);

    public async Task<bool> RemoveAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        await BlobCache.Secure.InvalidateObject<T>(key).ToTask(cancellationToken);
        return await BlobCache.Secure.Vacuum().SelectMany(_ => Observable.Return(true)).Catch(Observable.Return(false)).ToTask(cancellationToken);
    }

    public async Task<bool> ClearAsync(CancellationToken cancellationToken = default)
    {
        await BlobCache.Secure.InvalidateAll().ToTask(cancellationToken);
        return await BlobCache.Secure.Vacuum().SelectMany(_ => Observable.Return(true)).Catch(Observable.Return(false)).ToTask(cancellationToken);
    }

    #endregion
}


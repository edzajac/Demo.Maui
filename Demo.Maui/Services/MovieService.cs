using Demo.Maui.Constants;
using Demo.Maui.Extensions;
using Demo.Maui.Models;
using Demo.Maui.Models.Responses;
using Demo.Maui.Repositories.Interfaces;
using Demo.Maui.Services.Base;
using Demo.Maui.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Maui.Services
{
	public class MovieService : BaseHttpClient, IMovieService
	{
        #region Fields

        private readonly ILogger _logger;
        private readonly ICacheRepository _cacheRepository;

        #endregion

        #region Constructor

        public MovieService(ICacheRepository cacheRepository,
                            AppConfig appConfig) : base(appConfig)
        {
            //TODO: inject logger here
            //
            // _logger = logger;

            _cacheRepository = cacheRepository;
        }

        #endregion

        #region Methods

        public async Task<ImageConfigurationResponse?> GetImageConfigurationAsync(bool forceRefresh = false)
        {
            try
            {
                var cache = await _cacheRepository.GetCacheEntityAsync<ImageConfigurationResponse>(CacheConstants.Keys.ImageConfiguration);
                if (!forceRefresh && cache != null) return cache;

                var uriBuilder = new UriBuilder(BaseAddress);

                uriBuilder.AppendUrlSegment("configuration");
                uriBuilder.Query = $"api_key={ApiKey}";

                var message = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

                var response = await RequestAsync<ImageConfigurationResponse>(message);

                return await _cacheRepository.UpdateCacheEntityAsync<ImageConfigurationResponse>(CacheConstants.Keys.ImageConfiguration, response.Data);
            }
            catch(Exception ex)
            {
                //TODO: track exception via _logger
            }

            return default;
        }

        public async Task<GenreResponse?> GetAvailableGenresAsync(bool forceRefresh = false)
        {
            try
            {
                var cache = await _cacheRepository.GetCacheEntityAsync<GenreResponse>(CacheConstants.Keys.Genres);
                if (!forceRefresh && cache != null) return cache;

                var uriBuilder = new UriBuilder(BaseAddress);

                uriBuilder.AppendUrlSegment("genre/movie/list");
                uriBuilder.Query = $"api_key={ApiKey}" +
                                    "&language=en-US";

                var message = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

                var response = await RequestAsync<GenreResponse>(message);

                return await _cacheRepository.UpdateCacheEntityAsync<GenreResponse>(CacheConstants.Keys.Genres, response.Data);
            }
            catch(Exception ex)
            {
                //TODO: track exception via _logger
            }

            return default;
        }

        public async Task<PopularFilmsResponse?> GetMostRecentPopularFilmsAsync(bool forceRefresh = false)
        {
            try
            {
                var cache = await _cacheRepository.GetCacheEntityAsync<PopularFilmsResponse>(CacheConstants.Keys.RecentPopularFilms);
                if (!forceRefresh && cache != null) return cache;

                var uriBuilder = new UriBuilder(BaseAddress);

                uriBuilder.AppendUrlSegment("discover/movie");
                uriBuilder.Query = $"api_key={ApiKey}" +
                                    "&sort_by=popularity.desc";

                var message = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

                var response = await RequestAsync<PopularFilmsResponse>(message);

                return await _cacheRepository.UpdateCacheEntityAsync<PopularFilmsResponse>(CacheConstants.Keys.RecentPopularFilms, response.Data);
            }
            catch(Exception ex)
            {
                //TODO: track exception via _logger
            }

            return default;
        }

        #endregion
    }
}


using Demo.Maui.Models.Responses;

namespace Demo.Maui.Services.Interfaces;

public interface IMovieService
{
    public Task<ImageConfigurationResponse?> GetImageConfigurationAsync(bool forceRefresh = false);
    public Task<GenreResponse?> GetAvailableGenresAsync(bool forceRefresh = false);
    public Task<PopularFilmsResponse?> GetMostRecentPopularFilmsAsync(bool forceRefresh = false);
}


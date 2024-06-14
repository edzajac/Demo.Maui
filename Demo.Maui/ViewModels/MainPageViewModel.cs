using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Demo.Maui.Constants;
using Demo.Maui.Models;
using Demo.Maui.Services.Interfaces;
using Demo.Maui.ViewModels.Base;

namespace Demo.Maui.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
	#region Fields

	private readonly IMovieService _movieService;

    private ImageConfiguration _currentImageConfig;

    private List<Film> _popularFilms;
    private List<Genre> _genres;

    [ObservableProperty]
    private ObservableCollection<Genre> _filmsByGenre;

    #endregion

    #region Constructor

    public MainPageViewModel(IMovieService movieService)
	{
		_movieService = movieService;

        _currentImageConfig = new();
        _popularFilms = new();
        _genres = new();
        _filmsByGenre = new();
	}

    #endregion

    #region Methods

    public override Task InitAsync() => LoadDataAsync(isOnIt: true);

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadDataAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private Task NavigateToAboutFilmPageAsync(int id)
    {
        var selectedFilm = _popularFilms.Where(film => film.id == id)
                                        .First();

        var param = new ShellNavigationQueryParameters
        {
            { NavigationConstants.Parameters.SelectedFilm, selectedFilm }
        };

        return Shell.Current.GoToAsync(NavigationConstants.Routes.AboutFilm, true, param);
    }
    
    private async Task LoadDataAsync(bool isOnIt = false)
    {
        try
        {
            if(isOnIt) IsLoading = true;

            List<Genre> filmsByGenre = null;

            var haveLatest = await GetGenresAndFilmsAsync();

            if(haveLatest)
            {
                List<string> posterSizes = _currentImageConfig.poster_sizes.ToList();

                string posterSize = posterSizes.Any() && posterSizes.Count >= 2 ? posterSizes[1] : "w154";

                _popularFilms.ForEach(film =>
                {
                    film.poster_url = $"{_currentImageConfig.base_url}{posterSize}{film.poster_path}";
                });

                filmsByGenre = new List<Genre>
                {
                    new Genre
                    {
                        id = 0,
                        name = "Popular",
                        associated_films = _popularFilms.OrderBy(film => film.release_date)
                                                        .ToList()
                    }
                };

                List<Genre> filmsByGenreQuery = _genres.Select(genre =>
                {
                    List<Film> films = _popularFilms.Select(film => film)
                                                    .Where(film => film.genre_ids.Contains(genre.id))
                                                    .Distinct()
                                                    .OrderBy(film => film.title)
                                                    .ToList();
                    genre.associated_films = films;

                    return genre;

                })
                .Where(genre => genre.associated_films.Any() && genre.associated_films.Count >= 3)
                .ToList();

                filmsByGenre.AddRange(filmsByGenreQuery);
            }

            if (filmsByGenre != null)
            {
                FilmsByGenre = new ObservableCollection<Genre>(filmsByGenre);
            }

        }
        catch (Exception ex)
        {
            //TODO: Handle logging
        }
        finally
        {
           if (isOnIt) IsLoading = false;
        }
    }

    #endregion

    #region Api Methods

    private async Task<bool> GetGenresAndFilmsAsync()
    {
        var imageConfigResponse = await _movieService.GetImageConfigurationAsync(IsRefreshing);
        var genresResponse = await _movieService.GetAvailableGenresAsync(IsRefreshing);
        var filmsResponse = await _movieService.GetMostRecentPopularFilmsAsync(IsRefreshing);

        _currentImageConfig = imageConfigResponse.images;
        _genres = genresResponse.genres.ToList();
        _popularFilms = filmsResponse.results.ToList();

        return _currentImageConfig != null
            && _genres != null && _genres.Any()
            && _popularFilms != null && _popularFilms.Any();
    }

    #endregion
}


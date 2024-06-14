using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Demo.Maui.Constants;
using Demo.Maui.Models;
using Demo.Maui.ViewModels.Base;

namespace Demo.Maui.ViewModels;

[QueryProperty(nameof(Film), NavigationConstants.Parameters.SelectedFilm)]
public partial class AboutFilmPageViewModel : BaseViewModel
{
    #region Fields

    [ObservableProperty]
    private Film _film;

    #endregion

    #region Constructor

    public AboutFilmPageViewModel() 
    {
        _film = new();
    }

    #endregion
}


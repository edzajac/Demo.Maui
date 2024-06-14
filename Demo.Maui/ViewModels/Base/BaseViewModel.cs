using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Demo.Maui.ViewModels.Base;

#nullable enable
public partial class BaseViewModel : ObservableObject
{
    #region Fields

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private Color _backgroundColor = Colors.White;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _isLoading;

    #endregion

    #region Constructor

    public BaseViewModel() { }

    #endregion

    #region Base Methods

    public virtual Task InitAsync() => Task.FromResult(true);

    public virtual void OnAppearing() { }

    public virtual void OnDisappearing() { }

    [RelayCommand]
    private Task PopAsync() => Shell.Current.GoToAsync("..", true);

    #endregion
}


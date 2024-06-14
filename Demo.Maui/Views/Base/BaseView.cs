using System;
using Demo.Maui.ViewModels.Base;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Demo.Maui.Views.Base;

#nullable enable
public class BaseView : ContentPage
{
    #region Fields & Properties

    private long _initialized;

    public readonly BaseViewModel ViewModel;

    #endregion

    #region Constructor

    public BaseView(BaseViewModel viewModel)
    {
        BindingContext = ViewModel = viewModel;
    }

    #endregion

    #region Methods

    protected override async void OnAppearing()
    {
        if (Interlocked.CompareExchange(ref _initialized, 1, 0) == 0)
        {
            await ViewModel?.InitAsync();
        }

        base.OnAppearing();

        ViewModel?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel?.OnDisappearing();
    }

    #endregion

}


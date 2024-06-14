using Demo.Maui.Constants;
using Demo.Maui.Views;

namespace Demo.Maui;

public partial class AppShell : Shell
{
    #region Constructor

    public AppShell()
	{
		InitializeComponent();

        // Register routing for navigation

        Routing.RegisterRoute(NavigationConstants.Routes.AboutFilm, typeof(AboutFilmPageView));
	}

    #endregion
}


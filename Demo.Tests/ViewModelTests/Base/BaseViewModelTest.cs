using System;
using Demo.Tests.MockServices;
using Demo.Maui.Services.Interfaces;

namespace Demo.Tests.ViewModelTests.Base;

public class BaseViewModelTest
{
    #region Fields

    protected readonly IMovieService MovieService;

    #endregion

    #region Constructors

    public BaseViewModelTest()
	{
        MovieService = new MockMovieService();
    }

    #endregion
}


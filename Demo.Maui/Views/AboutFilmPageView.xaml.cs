using Demo.Maui.ViewModels;
using Demo.Maui.Views.Base;

namespace Demo.Maui.Views;

public partial class AboutFilmPageView : BaseView
{
	public AboutFilmPageView(AboutFilmPageViewModel viewModel): base(viewModel)
	{
		InitializeComponent();
	}
}

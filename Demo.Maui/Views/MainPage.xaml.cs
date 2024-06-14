using Demo.Maui.ViewModels;
using Demo.Maui.Views.Base;

namespace Demo.Maui.Views;

public partial class MainPage : BaseView
{
	public MainPage(MainPageViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

}

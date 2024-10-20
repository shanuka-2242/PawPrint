using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class HomeView : ContentPage
{
    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
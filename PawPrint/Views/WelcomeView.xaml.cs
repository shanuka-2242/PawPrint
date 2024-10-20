using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class WelcomeView : ContentPage
{
    public WelcomeView(WelcomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
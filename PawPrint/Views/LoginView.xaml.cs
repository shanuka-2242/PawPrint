using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
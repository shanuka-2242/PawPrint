using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class SignUpView : ContentPage
{
    public SignUpView(SignUpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
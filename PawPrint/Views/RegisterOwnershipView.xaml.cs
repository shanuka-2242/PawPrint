using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class RegisterOwnershipView : ContentPage
{
    public RegisterOwnershipView(RegisterOwnershipViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class VerifyOwnershipView : ContentPage
{
    public VerifyOwnershipView(VerifyOwnershipViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
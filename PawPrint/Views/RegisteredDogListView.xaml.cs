using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class RegisteredDogListView : ContentPage
{
    public RegisteredDogListView(RegisteredDogListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class RegisterOwnershipView : ContentPage
{
    private readonly RegisterOwnershipViewModel _viewModel;

    public RegisterOwnershipView(RegisterOwnershipViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadDataCommand.CanExecute(null))
            _viewModel.LoadDataCommand.Execute(null);
    }
}
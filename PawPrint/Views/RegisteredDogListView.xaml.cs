using PawPrint.ViewModels;

namespace PawPrint.Views;

public partial class RegisteredDogListView : ContentPage
{
    private readonly RegisteredDogListViewModel _viewModel;

    public RegisteredDogListView(RegisteredDogListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadRegisteredDogsImagesCommand.CanExecute(null))
            _viewModel.LoadRegisteredDogsImagesCommand.Execute(null);
    }
}
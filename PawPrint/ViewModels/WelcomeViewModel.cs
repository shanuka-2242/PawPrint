using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class WelcomeViewModel : ObservableObject
{
    public WelcomeViewModel()
    {
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
    }

    [ObservableProperty]
    private string copyrightText;

    [RelayCommand]
    async Task Register()
    {
        await Shell.Current.GoToAsync(nameof(LoginView));
    }

    [RelayCommand]
    async Task Verify()
    {
        await Shell.Current.GoToAsync(nameof(VerifyOwnershipView));
    }
}

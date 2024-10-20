using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    public LoginViewModel()
    {
        SetCopyright();
    }

    [ObservableProperty]
    private string copyrightText;

    void SetCopyright()
    {
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task GoToSignUpPage()
    {
        await Shell.Current.GoToAsync(nameof(SignUpView));
    }
}

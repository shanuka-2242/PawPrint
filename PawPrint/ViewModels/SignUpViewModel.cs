using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    public SignUpViewModel()
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
    async Task SignUp()
    {
        await Shell.Current.GoToAsync($"//{nameof(RegisterOwnershipView)}");
    }
}

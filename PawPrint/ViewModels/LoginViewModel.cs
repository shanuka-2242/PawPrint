using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Services.AuthenticateService;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthenticateService _authenticateService;

    public LoginViewModel(IAuthenticateService authenticateService)
    {
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
        _authenticateService = authenticateService;
    }

    #region Required Property List

    [ObservableProperty]
    private string copyrightText;

    [ObservableProperty]
    private string enteredNIC;

    [ObservableProperty]
    private string enteredPassword;

    [ObservableProperty]
    private bool isBusy;

    #endregion

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

    [RelayCommand]
    async Task LogIn()
    {
        try
        {
            if (EnteredNIC != null && EnteredPassword != null)
            {
                IsBusy = true;
                var loggedinUser = await _authenticateService.GetOwnerByNICAsync(EnteredNIC);
                IsBusy = false;

                if (loggedinUser != null)
                {
                    if (EnteredPassword == loggedinUser.Password)
                    {
                        var param = new Dictionary<string, object>
                        {
                            { "LoggedInUserNIC", loggedinUser.NIC }
                        };

                        await Shell.Current.GoToAsync($"//{nameof(RegisterOwnershipView)}", param);
                    }
                    else
                    {
                        await Toast.Make("Entered credentials must be wrong please chack again", ToastDuration.Long, 14).Show();
                    }
                }
                else
                {
                    await Toast.Make("Entered credentials must be wrong or user doesn't exists", ToastDuration.Long, 14).Show();
                }
            }
            else
            {
                await Toast.Make("Entry fields cannot be empty", ToastDuration.Long, 14).Show();
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while logging into the application", ToastDuration.Long, 14).Show();
        }
    }
}

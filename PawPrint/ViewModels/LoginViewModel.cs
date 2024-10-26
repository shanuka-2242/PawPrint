using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Services.AuthenticateService;
using PawPrint.Services.DialogService;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IAuthenticateService _authenticateService;

    public LoginViewModel(IDialogService dialogService, IAuthenticateService authenticateService)
    {
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
        _dialogService = dialogService;
        _authenticateService = authenticateService;
    }

    #region Required Property List

    [ObservableProperty]
    private string copyrightText;

    [ObservableProperty]
    private string enteredNIC;

    [ObservableProperty]
    private string enteredPassword;

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
            if (EnteredNIC != null & EnteredPassword != null)
            {
                var loggedinUser = await _authenticateService.GetOwnerByNICAsync(EnteredNIC);
                if (loggedinUser != null)
                {
                    if (EnteredPassword == loggedinUser.Password)
                    {
                        var param = new ShellNavigationQueryParameters
                        {
                            { "LoggedInUserNIC", loggedinUser.NIC }
                        };

                        await Shell.Current.GoToAsync($"//{nameof(RegisterOwnershipView)}", param);
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Information", "Entered credentials must be wrong please chack again", "OK");
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Information", "Entered credentials must be wrong or user doesn't exists.", "OK");
                }
            }
            else
            {
                await _dialogService.ShowAlertAsync("Information", "Entry fields cannot be empty.", "OK");
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while logging into the application", "OK");
        }
    }
}

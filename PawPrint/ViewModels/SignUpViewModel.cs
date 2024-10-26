using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Services.AuthenticateService;
using PawPrint.Services.DialogService;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IAuthenticateService _authenticateService;

    public SignUpViewModel(IDialogService dialogService, IAuthenticateService authenticateService)
    {
        _dialogService = dialogService;
        _authenticateService = authenticateService;
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
    }

    #region Required Property List

    [ObservableProperty]
    private string copyrightText;

    [ObservableProperty]
    private string enteredNIC;

    [ObservableProperty]
    private string enteredFullName;

    [ObservableProperty]
    private string enteredPhone;

    [ObservableProperty]
    private string enteredEmail;

    [ObservableProperty]
    private string enteredCurrentAddress;

    [ObservableProperty]
    private string enteredConfirmPassword;

    [ObservableProperty]
    private string enteredPassword;

    #endregion

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task SignUp()
    {
        try
        {
            if (EnteredNIC != null || EnteredFullName != null || EnteredPhone != null || EnteredEmail != null
                || EnteredCurrentAddress != null || EnteredConfirmPassword != null || EnteredPassword != null)
            {
                var alreadyUser = await _authenticateService.GetOwnerByNICAsync(EnteredNIC);
                if (alreadyUser == null) 
                {
                    if (EnteredConfirmPassword == EnteredPassword)
                    {
                        using var form = new MultipartFormDataContent
                    {
                        { new StringContent(EnteredNIC), "nic" },
                        { new StringContent(EnteredFullName), "full_name" },
                        { new StringContent(EnteredPhone), "phone" },
                        { new StringContent(EnteredEmail), "email" },
                        { new StringContent(EnteredCurrentAddress), "current_address" },
                        { new StringContent(EnteredPassword), "password" }
                    };

                        var result = await _authenticateService.SignUpOwnerAsync(form);
                        if (result)
                        {
                            var param = new ShellNavigationQueryParameters
                        {
                            { "LoggedInUserNIC", EnteredNIC }
                        };

                            await Shell.Current.GoToAsync($"//{nameof(RegisterOwnershipView)}", param);
                        }
                        else
                        {
                            await _dialogService.ShowAlertAsync("Information", "Signing up failed.", "OK");
                        }
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Information", "Confirm Password and Password values are not equal to each other.", "OK");
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Information", "An owner already logged in under this NIC number.", "OK");
                }
            }
            else
            {
                await _dialogService.ShowAlertAsync("Information", "Entry fields cannot be empty.", "OK");
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while signing into the application", "OK");
        }
    }
}

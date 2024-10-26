using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
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
    private string enteredConfirmPassword;

    [ObservableProperty]
    private Owner owner = new();

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
            if (Owner.NIC != null & Owner.FullName != null & Owner.Phone != null & Owner.Email != null
                & Owner.CurrentAddress != null & Owner.Password != null & EnteredConfirmPassword != null)
            {
                var alreadyUser = await _authenticateService.GetOwnerByNICAsync(Owner.NIC);
                if (alreadyUser == null)
                {
                    if (EnteredConfirmPassword == Owner.Password)
                    {
                        using var form = new MultipartFormDataContent
                        {
                            { new StringContent(Owner.NIC), "nic" },
                            { new StringContent(Owner.FullName), "full_name" },
                            { new StringContent(Owner.Phone), "phone" },
                            { new StringContent(Owner.Email), "email" },
                            { new StringContent(Owner.CurrentAddress), "current_address" },
                            { new StringContent(Owner.Password), "password" }
                        };

                        var result = await _authenticateService.SignUpOwnerAsync(form);
                        if (result)
                        {
                            var param = new ShellNavigationQueryParameters
                           {
                               { "LoggedInUserNIC", Owner.NIC }
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

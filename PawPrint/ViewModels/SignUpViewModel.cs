using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.AuthenticateService;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    private readonly IAuthenticateService _authenticateService;

    public SignUpViewModel(IAuthenticateService authenticateService)
    {
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
            if (Owner.NIC != null && Owner.FullName != null && Owner.Phone != null && Owner.Email != null
                && Owner.CurrentAddress != null && Owner.Password != null && EnteredConfirmPassword != null)
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
                            var param = new Dictionary<string, object>
                            {
                                { "LoggedInUserNIC", Owner.NIC }
                            };

                            await Shell.Current.GoToAsync($"//{nameof(RegisterOwnershipView)}", param);
                        }
                        else
                        {
                            await Toast.Make("Signing up failed.", ToastDuration.Long, 14).Show();
                        }
                    }
                    else
                    {
                        await Toast.Make("Confirm Password and Password values are not equal to each other.", ToastDuration.Long, 14).Show();
                    }
                }
                else
                {
                    await Toast.Make("An owner already logged in under this NIC number.", ToastDuration.Long, 14).Show();
                }
            }
            else
            {
                await Toast.Make("Entry fields cannot be empty.", ToastDuration.Long, 14).Show();
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while signing into the application.", ToastDuration.Long, 14).Show();
        }
    }
}

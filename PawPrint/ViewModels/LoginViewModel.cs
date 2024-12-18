﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Services.AuthenticateService;
using PawPrint.Views;
using System.Text.RegularExpressions;

namespace PawPrint.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthenticateService _authenticateService;

    public LoginViewModel(IAuthenticateService authenticateService)
    {
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
        _authenticateService = authenticateService;
        PasswordVisibilityImageSource = ImageSource.FromFile("visibility.png");
    }

    #region Regex Fields

    private string _nicRegex = @"^(?:19|20)?\d{2}[0-9]{10}|[0-9]{9}[x|X|v|V]$";

    #endregion

    #region Required Property List

    [ObservableProperty]
    ImageSource passwordVisibilityImageSource;

    [ObservableProperty]
    private string copyrightText;

    [ObservableProperty]
    private string enteredNIC;

    [ObservableProperty]
    private string enteredPassword;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isPassword = true;

    #endregion

    [RelayCommand]
    private void VisiblePassword()
    {
        try
        {
            if (IsPassword)
            {
                IsPassword = false;
                PasswordVisibilityImageSource = ImageSource.FromFile("visibility_off.png");
            }
            else
            {
                IsPassword = true;
                PasswordVisibilityImageSource = ImageSource.FromFile("visibility.png");
            }
        }
        catch (Exception)
        { }
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

    [RelayCommand]
    async Task LogIn()
    {
        try
        {
            if (EnteredNIC != null && EnteredPassword != null)
            {
                if (Regex.IsMatch(EnteredNIC, _nicRegex))
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
                    await Toast.Make("Entered NIC number isn't in the proper format to be used in Sri Lanka", ToastDuration.Long, 14).Show();
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

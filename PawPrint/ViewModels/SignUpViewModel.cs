using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.AuthenticateService;
using PawPrint.Views;
using System.Text.RegularExpressions;

namespace PawPrint.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    private readonly IAuthenticateService _authenticateService;

    public SignUpViewModel(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
        CopyrightText = $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
        PasswordVisibilityImageSource = ImageSource.FromFile("visibility.png");
        ConfirmPasswordVisibilityImageSource = ImageSource.FromFile("visibility.png");
    }

    #region Regex Fields

    private string _fullNameRegex = @"^[a-zA-Z]+(?:\s[a-zA-Z.]+)*$";
    private string _nicRegex = @"^(?:19|20)?\d{2}[0-9]{10}|[0-9]{9}[x|X|v|V]$";
    private string _emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private string _phoneRegex = @"^(?:\+94|94|0)?7[0-9]{8}$";

    #endregion

    #region Required Property List

    [ObservableProperty]
    private bool isPassword = true;

    [ObservableProperty]
    ImageSource passwordVisibilityImageSource;

    [ObservableProperty]
    private bool isConfirmPassword = true;

    [ObservableProperty]
    ImageSource confirmPasswordVisibilityImageSource;

    [ObservableProperty]
    private string copyrightText;

    [ObservableProperty]
    private string enteredConfirmPassword;

    [ObservableProperty]
    private Owner owner = new();

    [ObservableProperty]
    private bool isBusy;

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
    private void VisibleConfirmPassword()
    {
        try
        {
            if (IsConfirmPassword)
            {
                IsConfirmPassword = false;
                ConfirmPasswordVisibilityImageSource = ImageSource.FromFile("visibility_off.png");
            }
            else
            {
                IsConfirmPassword = true;
                ConfirmPasswordVisibilityImageSource = ImageSource.FromFile("visibility.png");
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
    async Task SignUp()
    {
        try
        {
            if (Owner.NIC != null && Owner.FullName != null && Owner.Phone != null && Owner.Email != null
                && Owner.CurrentAddress != null && Owner.Password != null && EnteredConfirmPassword != null)
            {
                if (Regex.IsMatch(Owner.FullName, _fullNameRegex))
                {
                    if (Regex.IsMatch(Owner.NIC, _nicRegex))
                    {
                        if (Regex.IsMatch(Owner.Phone, _phoneRegex))
                        {
                            if (Regex.IsMatch(Owner.Email, _emailRegex))
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

                                        IsBusy = true;
                                        var result = await _authenticateService.SignUpOwnerAsync(form);
                                        IsBusy = false;

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
                                            await Toast.Make("Signing up failed", ToastDuration.Long, 14).Show();
                                        }
                                    }
                                    else
                                    {
                                        await Toast.Make("Confirm Password and Password values are not equal to each other", ToastDuration.Long, 14).Show();
                                    }
                                }
                                else
                                {
                                    await Toast.Make("An owner already logged in under this NIC number", ToastDuration.Long, 14).Show();
                                }
                            }
                            else
                            {
                                await Toast.Make("Entered email address isn't in the proper format", ToastDuration.Long, 14).Show();
                            }
                        }
                        else
                        {
                            await Toast.Make("Entered phone number isn't in the proper format to be used in Sri Lanka", ToastDuration.Long, 14).Show();
                        }
                    }
                    else
                    {
                        await Toast.Make("Entered NIC number isn't in the proper format to be used in Sri Lanka", ToastDuration.Long, 14).Show();
                    }
                }
                else
                {
                    await Toast.Make("Entered fullname isn't in correct format", ToastDuration.Long, 14).Show();
                }
            }
            else
            {
                await Toast.Make("Entry fields cannot be empty", ToastDuration.Long, 14).Show();
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while signing into the application", ToastDuration.Long, 14).Show();
        }
    }
}

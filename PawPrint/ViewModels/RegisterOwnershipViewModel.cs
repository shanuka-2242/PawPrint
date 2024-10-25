using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.DialogService;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class RegisterOwnershipViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;

    public RegisterOwnershipViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [ObservableProperty]
    private Dog registeredDog;

    [ObservableProperty]
    private FileResult selectedDogImage;

    [ObservableProperty]
    private string selectedDogImageName;

    [ObservableProperty]
    private FileResult selectedDogNoseImage;

    [ObservableProperty]
    private string selectedDogNoseImageName;

    private string nic = "2000";

    [RelayCommand]
    async Task LogOut()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
    }

    [RelayCommand]
    async Task AttachDogImage()
    {
        try
        {
            SelectedDogImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogImage != null)
            {
                SelectedDogImageName = $"Image - {SelectedDogImage.FileName}";
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting dog image.", "OK");
        }
    }

    [RelayCommand]
    async Task AttachDogNoseImage()
    {
        try
        {
            SelectedDogNoseImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogNoseImage != null)
            {
                SelectedDogNoseImageName = $"Image - {SelectedDogNoseImage.FileName}";
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting dog nose image.", "OK");
        }
    }

    [RelayCommand]
    async Task RegisterDog()
    {
        try
        {

        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while registering dog.", "OK");
        }
    }
}

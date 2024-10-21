using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Views;

namespace PawPrint.ViewModels;

public partial class RegisterOwnershipViewModel : ObservableObject
{
    [ObservableProperty]
    private Dog registeredDog;

    [ObservableProperty]
    private FileResult dogImage;

    [ObservableProperty]
    private string dogImageName;

    [ObservableProperty]
    private FileResult dogNoseImage;

    [ObservableProperty]
    private string dogNoseImageName;

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
            DogImage = await MediaPicker.PickPhotoAsync();
            if (DogImage != null)
            {
                DogImageName = $"Image - {DogImage.FileName}";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    [RelayCommand]
    async Task AttachDogNoseImage()
    {

    }

    [RelayCommand]
    async Task RegisterDog()
    {
        try
        {

        }
        catch (Exception)
        {

        }
    }
}

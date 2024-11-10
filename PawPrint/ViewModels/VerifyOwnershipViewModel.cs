using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.VerifyOwnershipService;

namespace PawPrint.ViewModels;

public partial class VerifyOwnershipViewModel : ObservableObject
{
    private readonly IVerifyOwnershipService _verifyOwnershipService;

    public VerifyOwnershipViewModel(IVerifyOwnershipService verifyOwnershipService)
    {
        _verifyOwnershipService = verifyOwnershipService;
        SelectedDogNoseImageSource = ImageSource.FromFile("add_photo.png");
        DogImageSource = ImageSource.FromFile("image_view.png");
    }

    #region Required Property List

    [ObservableProperty]
    private VerifiedInfomation verifiedInfomation;

    private FileResult SelectedDogNoseImage;

    [ObservableProperty]
    private ImageSource selectedDogNoseImageSource;

    [ObservableProperty]
    private ImageSource dogImageSource;

    [ObservableProperty]
    private bool isEnabled;

    [ObservableProperty]
    private bool isBusy;

    #endregion

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task SelectImage()
    {
        try
        {
            SelectedDogNoseImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogNoseImage != null)
            {
                VerifiedInfomation = null;
                DogImageSource = ImageSource.FromFile("image_view.png");
                IsEnabled = true;
                var stream = await SelectedDogNoseImage.OpenReadAsync();
                SelectedDogNoseImageSource = ImageSource.FromStream(() => stream);
            }
            else
            {
                SelectedDogNoseImageSource = ImageSource.FromFile("add_photo.png");
                DogImageSource = ImageSource.FromFile("image_view.png");
                IsEnabled = false;
                VerifiedInfomation = null;
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while selecting an image", ToastDuration.Long, 14).Show();
        }
    }

    [RelayCommand]
    async Task VerifyOwnership()
    {
        try
        {
            var streamImg = await SelectedDogNoseImage.OpenReadAsync();
            if (streamImg != null)
            {
                using var form = new MultipartFormDataContent
                {
                    {
                        new StreamContent(streamImg),
                        "file",
                        SelectedDogNoseImage.FileName
                    }
                };

                // API Call (Request)
                IsBusy = true;
                var result = await _verifyOwnershipService.GetOwnerVerifiedInfoAsync(form);
                IsBusy = false;

                if (result.Dog == null && result.Owner == null)
                {
                    await Toast.Make("A dog is not recorded in our database with this biometric", ToastDuration.Long, 14).Show();
                }
                else
                {
                    VerifiedInfomation = result;
                    DogImageSource = VerifiedInfomation.Dog.DogImageSource;
                }
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured, Please try again", ToastDuration.Long, 14).Show();
        }
    }
}

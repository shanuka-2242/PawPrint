using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.DialogService;
using PawPrint.Services.VerifyOwnershipService;

namespace PawPrint.ViewModels;

public partial class VerifyOwnershipViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IVerifyOwnershipService _verifyOwnershipService;

    public VerifyOwnershipViewModel(IDialogService dialogService, IVerifyOwnershipService verifyOwnershipService)
    {
        _dialogService = dialogService;
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
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting an image.", "OK");
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
                var result = await _verifyOwnershipService.GetOwnerVerifiedInfoAsync(form);

                if (result.Dog == null && result.Owner == null)
                {
                    await _dialogService.ShowAlertAsync("Information", "A dog is not recorded in our database with this biometric.", "OK");
                }
                else
                {
                    VerifiedInfomation = result;
                    var dogImage = await _verifyOwnershipService.GetDogImageAsync(result.Dog.EntryID);
                    if (dogImage != null)
                    {
                        var stream = new MemoryStream(dogImage);
                        DogImageSource = ImageSource.FromStream(() => stream);
                    }
                }
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occurred while selecting an Image.", "OK");
        }
    }
}

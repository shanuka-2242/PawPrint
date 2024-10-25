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
    }

    [ObservableProperty]
    private VerifiedInfomation verifiedInfomation;

    [ObservableProperty]
    private FileResult selectedDogNoseImage;

    [ObservableProperty]
    private ImageSource selectedDogNoseImageSource;

    [ObservableProperty]
    private FileResult dogImage;
    
    [ObservableProperty]
    private ImageSource dogImageSource;

    [ObservableProperty]
    private bool isEnabled;

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
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting an image.", "OK");
        }
    }

    [RelayCommand]
    async Task VerifyOwnership()
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await (await SelectedDogNoseImage.OpenReadAsync()).CopyToAsync(memoryStream);

            using var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(memoryStream.ToArray()), "file", SelectedDogNoseImage.FileName }
            };

            var result = await _verifyOwnershipService.GetOwnerVerifiedInfoAsync(content);

            if (result.Dog == null && result.Owner == null)
            {
                await _dialogService.ShowAlertAsync("Information", "No dog is recorded in our database with this biometric.", "OK");
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
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occurred while selecting an Image.", "OK");
        }
    }
}

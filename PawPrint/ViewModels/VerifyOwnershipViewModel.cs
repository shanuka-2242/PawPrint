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
    private ImageSource selectedImageSource;

    [ObservableProperty]
    private FileResult selectedImageFile;

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
            //SelectedImageFile = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            //{
            //    Title = "Select a dog's nose image",
            //});

            SelectedImageFile = await MediaPicker.PickPhotoAsync();
            if (SelectedImageFile != null)
            {
                IsEnabled = true;
                var stream = await SelectedImageFile.OpenReadAsync();
                SelectedImageSource = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting an Image.", "OK");
        }
    }

    [RelayCommand]
    async Task VerifyOwnership()
    {
        try
        {
            var stream = await SelectedImageFile.OpenReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            using var content = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(imageBytes);
            content.Add(byteArrayContent, "file", SelectedImageFile.FileName);

            var result = await _verifyOwnershipService.GetOwnerVerifiedInfoAsync(content);
            if (result == null)
            {
                await _dialogService.ShowAlertAsync("Information", "No dog is recorded in our database with this biometric.", "OK");
            }
            else
            {
                VerifiedInfomation = result;
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while selecting an Image.", "OK");
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Services.DialogService;
using PawPrint.Services.RegisterOwnershipService;
using PawPrint.Views;
using System.Net.Http.Headers;

namespace PawPrint.ViewModels;

[QueryProperty(nameof(LoggedInUserNIC), "LoggedInUserNIC")]
public partial class RegisterOwnershipViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IRegisterOwnershipService _registerOwnershipService;

    public RegisterOwnershipViewModel(IDialogService dialogService, IRegisterOwnershipService registerOwnershipService)
    {
        _registerOwnershipService = registerOwnershipService;
        _dialogService = dialogService;
    }

    #region Query Param

    string loggedInUserNIC;
    public string LoggedInUserNIC
    {
        get => loggedInUserNIC;
        set
        {
            loggedInUserNIC = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Required Property List

    public Stream DogImage { get; set; }

    public Stream NoseImage { get; set; }

    [ObservableProperty]
    public string noseImageName;

    [ObservableProperty]
    public string dogImageName;

    [ObservableProperty]
    public string dogName;

    [ObservableProperty]
    public string breed;

    [ObservableProperty]
    public string age;

    #endregion

    [RelayCommand]
    async Task LogOut()
    {
        await Shell.Current.GoToAsync($"//{nameof(WelcomeView)}");
    }

    [RelayCommand]
    async Task AttachDogImage()
    {
        try
        {
            var SelectedDogImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogImage != null)
            {
                DogImageName = $"Image - {SelectedDogImage.FileName}";
                DogImage = await SelectedDogImage.OpenReadAsync();
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
            var SelectedDogNoseImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogNoseImage != null)
            {
                NoseImageName = $"Image - {SelectedDogNoseImage.FileName}";
                NoseImage = await SelectedDogNoseImage.OpenReadAsync();
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
            if (DogName != null || Age != null || Breed != null
                || NoseImage != null || DogImage != null)
            {
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(DogName), "dog_name" },
                    { new StringContent(Breed), "breed" },
                    { new StringContent(Age), "age" },
                    { new StringContent(LoggedInUserNIC), "owner_nic" }
                };

                var noseImageContent = new StreamContent(NoseImage);
                noseImageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(noseImageContent, "nose_image", NoseImageName);

                var dogImageContent = new StreamContent(DogImage);
                dogImageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(dogImageContent, "dog_image", DogImageName);

                var result = await _registerOwnershipService.RegisterOwnershipAsync(form);
                if (result)
                {
                    await _dialogService.ShowAlertAsync("Information", "Your dog registered into our system sucessfully.", "OK");
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Information", "Error occured while registering the dog.", "OK");
                }
            }
            else
            {
                await _dialogService.ShowAlertAsync("Information", "Entry fields cannot be empty.", "OK");
            }
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("Information", "Error occured while registering dog.", "OK");
        }
    }
}

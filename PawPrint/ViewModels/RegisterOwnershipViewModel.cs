using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.RegisterOwnershipService;
using PawPrint.Views;
using System.Net.Http.Headers;

namespace PawPrint.ViewModels;

public partial class RegisterOwnershipViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRegisterOwnershipService _registerOwnershipService;

    public RegisterOwnershipViewModel(IRegisterOwnershipService registerOwnershipService)
    {
        _registerOwnershipService = registerOwnershipService;
    }

    #region Required Property List

    public Stream DogImage { get; set; }

    public List<Dog> RegisteredDogList { get; set; }

    public string LoggedInUserNIC { get; set; }

    [ObservableProperty]
    public string dogImageName;

    public Stream NoseImage { get; set; }

    [ObservableProperty]
    public string noseImageName;

    #region Age (Years & Months Properties)

    [ObservableProperty]
    public string years;

    [ObservableProperty]
    public string months;

    #endregion

    [ObservableProperty]
    public Dog dog = new();

    [ObservableProperty]
    public bool viewRegisteredDogList;

    #endregion

    [RelayCommand]
    async Task GoToRegisteredDogListView()
    {
        var param = new Dictionary<string, object>
        {
            { "RegisteredDogList", RegisteredDogList },
            { "LoggedInUserNIC", LoggedInUserNIC }
        };
        await Shell.Current.GoToAsync(nameof(RegisteredDogListView), param);
    }

    private async Task LoadDataAsync()
    {
        var registeredDogs = await _registerOwnershipService.GetRegisteredDogsByOwnerNICAsync(LoggedInUserNIC);
        if (registeredDogs != null)
        {
            ViewRegisteredDogList = true;
            RegisteredDogList = registeredDogs;
        }
        else
        {
            ViewRegisteredDogList = false;
            RegisteredDogList = null;
        }
    }

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
            var SelectedDogImage = await MediaPicker.PickPhotoAsync();
            if (SelectedDogImage != null)
            {
                DogImageName = $"Image - {SelectedDogImage.FileName}";
                DogImage = await SelectedDogImage.OpenReadAsync();
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while selecting dog image.", ToastDuration.Long, 14).Show();
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
            await Toast.Make("Error occured while selecting dog nose image.", ToastDuration.Long, 14).Show();
        }
    }

    private void ClearFields()
    {
        DogImage = null;
        DogImageName = null;
        Dog = new();
        NoseImage = null;
        NoseImageName = null;
        Years = null;
        Months = null;
    }

    [RelayCommand]
    async Task RegisterDog()
    {
        try
        {
            if (Dog.Name != null && Years != null && Months != null && Dog.Breed != null && NoseImage != null && DogImage != null)
            {
                //Set dog age
                Dog.Age = $"{Years} Years, {Months} Months";
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(Dog.Name), "dog_name" },
                    { new StringContent(Dog.Breed), "breed" },
                    { new StringContent(Dog.Age), "age" },
                    { new StringContent(LoggedInUserNIC), "owner_nic" }
                };

                var noseImageContent = new StreamContent(NoseImage);
                noseImageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(noseImageContent, "nose_image", NoseImageName);

                var dogImageContent = new StreamContent(DogImage);
                dogImageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(dogImageContent, "dog_image", DogImageName);

                var result = await _registerOwnershipService.RegisterOwnershipAsync(form);
                if (result == 200)
                {
                    await Toast.Make("Your dog registered into our system sucessfully.", ToastDuration.Long, 14).Show();
                }
                else if (result == 400)
                {
                    await Toast.Make("This dog is already registered in our system.", ToastDuration.Long, 14).Show();
                }
                else
                {
                    await Toast.Make("Error occured while registering the dog.", ToastDuration.Long, 14).Show();
                }
                await LoadDataAsync();
                ClearFields();
            }
            else
            {
                await Toast.Make("Entry fields cannot be empty.", ToastDuration.Long, 14).Show();
            }
        }
        catch (Exception)
        {
            await Toast.Make("Error occured while registering dog.", ToastDuration.Long, 14).Show();
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        LoggedInUserNIC = query["LoggedInUserNIC"] as string;
        OnPropertyChanged(nameof(LoggedInUserNIC));

        if (LoggedInUserNIC != null)
        {
            await LoadDataAsync();
        }
    }
}

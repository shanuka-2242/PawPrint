using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
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
        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
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

    public List<Dog> RegisteredDogList { get; set; }

    public IAsyncRelayCommand LoadDataCommand { get; }

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
        var registeredDogs = await _registerOwnershipService.GetRegisteredDogsByOwnerNICAsync("200023802470");
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
                    //{ new StringContent(LoggedInUserNIC), "owner_nic" }
                    { new StringContent("200023802470"), "owner_nic" }
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
                    await _dialogService.ShowAlertAsync("Information", "Your dog registered into our system sucessfully.", "OK");
                }
                else if (result == 400)
                {
                    await _dialogService.ShowAlertAsync("Information", "This dog is already registered in our system.", "OK");
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Information", "Error occured while registering the dog.", "OK");
                }
                await LoadDataAsync();
                ClearFields();
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

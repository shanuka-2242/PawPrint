﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.DialogService;
using PawPrint.Services.RegisteredDogListService;
using PawPrint.Services.RegisterOwnershipService;

namespace PawPrint.ViewModels
{
    public partial class RegisteredDogListViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IRegisteredDogListService _registeredDogListService;
        private readonly IRegisterOwnershipService _registerOwnershipService;
        private readonly IDialogService _dialogService;

        #region Required Property List

        [ObservableProperty]
        public List<Dog> registeredDogList = [];

        public List<ImageSource> registeredDogImageListByNIC = [];

        public string LoggedInUserNIC { get; private set; }

        public IAsyncRelayCommand LoadRegisteredDogsImagesCommand { get; }

        #endregion

        public RegisteredDogListViewModel(IDialogService dialogService, IRegisteredDogListService registeredDogListService, IRegisterOwnershipService registerOwnershipService)
        {
            _dialogService = dialogService;
            _registeredDogListService = registeredDogListService;
            _registerOwnershipService = registerOwnershipService;
            LoadRegisteredDogsImagesCommand = new AsyncRelayCommand(LoadRegisteredImagesAsync);
        }

        private async Task LoadRegisteredImagesAsync()
        {
            var registeredDogsImages = await _registeredDogListService.GetRegisteredDogImagesByNICAsync("200023802470");
            if (registeredDogsImages != null)
            {
                if (registeredDogsImages.Count == RegisteredDogList.Count)
                {
                    for (int i = 0; i < RegisteredDogList.Count; i++)
                    {
                        //RegisteredDogList[i].DogImage = registeredDogsImages[i];
                    }
                }
            }
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task RemoveRegiteredDog(Dog dog)
        {
            try
            {
                if (dog != null)
                {
                    var result = await _registeredDogListService.RemoveRegisteredDogAsync(dog.EntryID);
                    if (result)
                    {
                        await _dialogService.ShowAlertAsync("Information", "Registered dog removed sucessfully.", "OK");
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Information", "Registered dog removing failed.", "OK");
                    }
                    RegisteredDogList = await _registerOwnershipService.GetRegisteredDogsByOwnerNICAsync("200023802470");
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowAlertAsync("Information", "Error occured while removing registered dog.", "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RegisteredDogList = query["RegisteredDogList"] as List<Dog>;
            OnPropertyChanged(nameof(RegisteredDogList));

            LoggedInUserNIC = query["LoggedInUserNIC"] as string;
            OnPropertyChanged(nameof(LoggedInUserNIC));
        }
    }
}
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;
using PawPrint.Services.RegisteredDogListService;
using PawPrint.Services.RegisterOwnershipService;

namespace PawPrint.ViewModels
{
    public partial class RegisteredDogListViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IRegisteredDogListService _registeredDogListService;
        private readonly IRegisterOwnershipService _registerOwnershipService;

        #region Required Property List

        [ObservableProperty]
        public List<Dog> registeredDogList;

        public string LoggedInUserNIC { get; private set; }

        #endregion

        public RegisteredDogListViewModel(IRegisteredDogListService registeredDogListService, IRegisterOwnershipService registerOwnershipService)
        {
            _registeredDogListService = registeredDogListService;
            _registerOwnershipService = registerOwnershipService;
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
                    var choice = await Application.Current.MainPage.DisplayAlert("Confirmation", $"Are you sure about removing '{dog.Name}' from the list?", "Yes", "Cancel");
                    if (choice)
                    {
                        var result = await _registeredDogListService.RemoveRegisteredDogAsync(dog.EntryID);
                        if (result)
                        {
                            await Toast.Make("Registered dog removed sucessfully", ToastDuration.Long, 14).Show();
                        }
                        else
                        {
                            await Toast.Make("Registered dog removing failed", ToastDuration.Long, 14).Show();
                        }
                        RegisteredDogList = await _registerOwnershipService.GetRegisteredDogsByOwnerNICAsync(LoggedInUserNIC);
                    }
                }
            }
            catch (Exception)
            {
                await Toast.Make("Error occured while removing registered dog", ToastDuration.Long, 14).Show();
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PawPrint.Models;

namespace PawPrint.ViewModels
{
    [QueryProperty(nameof(RegisteredDogList), "RegisteredDogList")]
    public partial class RegisteredDogListViewModel : ObservableObject
    {
        #region Query Param

        List<Dog> registeredDogList;
        public List<Dog> RegisteredDogList
        {
            get => registeredDogList;
            set
            {
                registeredDogList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

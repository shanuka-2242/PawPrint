namespace PawPrint.Services.DialogService;

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message, string cancel)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }
}

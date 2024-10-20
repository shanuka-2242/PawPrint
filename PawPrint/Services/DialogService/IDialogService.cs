namespace PawPrint.Services.DialogService;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel);
}
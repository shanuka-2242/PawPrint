
namespace PawPrint.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();

        copyNoticeLable.Text = GetCopyRightText();
    }

    /// <summary>
    /// Load copyright text
    /// </summary>
    private string GetCopyRightText()
    {
        return $"® {DateTime.Now.Year} Paw Print Sri Lanka. All rights reserved";
    }
}
namespace PawPrint.Views;

public partial class WelcomeView : ContentPage
{
	public WelcomeView()
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

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
    }
}
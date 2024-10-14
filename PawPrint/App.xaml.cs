using PawPrint.CustomControls;

namespace PawPrint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            //Custom Entry Code Line (Entry without underline)
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(CustomEntry), (Handler, View) =>
            {
                if (View is CustomEntry)
                {
#if ANDROID
                    Handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
                }
            });
        }
    }
}
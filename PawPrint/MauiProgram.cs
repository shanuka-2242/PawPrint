using Microsoft.Extensions.Logging;

namespace PawPrint
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    //Signika font register
                    fonts.AddFont("Signika-Bold.ttf", "SignikaBold");
                    fonts.AddFont("Signika-Light.ttf", "SignikaLight");
                    fonts.AddFont("Signika-Medium.ttf", "SignikaMedium");
                    fonts.AddFont("Signika-Regular.ttf", "SignikaRegular");
                    fonts.AddFont("Signika-SemiBold.ttf", "SignikaSemiBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

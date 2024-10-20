﻿using Microsoft.Extensions.Logging;
using PawPrint.Services.DialogService;
using PawPrint.Services.VerifyOwnershipService;
using PawPrint.ViewModels;
using PawPrint.Views;

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

            builder.Services.AddSingleton<WelcomeView>();
            builder.Services.AddSingleton<WelcomeViewModel>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<SignUpView>();
            builder.Services.AddTransient<SignUpViewModel>();

            builder.Services.AddTransient<VerifyOwnershipView>();
            builder.Services.AddTransient<VerifyOwnershipViewModel>();

            builder.Services.AddSingleton<IDialogService, DialogService>();

            builder.Services.AddHttpClient<IVerifyOwnershipService, VerifyOwnershipService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

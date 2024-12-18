﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PawPrint.Services.AuthenticateService;
using PawPrint.Services.RegisteredDogListService;
using PawPrint.Services.RegisterOwnershipService;
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
                .UseMauiCommunityToolkit()
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

            builder.Services.AddTransient<RegisterOwnershipView>();
            builder.Services.AddTransient<RegisterOwnershipViewModel>();

            builder.Services.AddTransient<RegisteredDogListView>();
            builder.Services.AddTransient<RegisteredDogListViewModel>();

            builder.Services.AddHttpClient<IVerifyOwnershipService, VerifyOwnershipService>();
            builder.Services.AddHttpClient<IAuthenticateService, AuthenticateService>();
            builder.Services.AddHttpClient<IRegisterOwnershipService, RegisterOwnershipService>();
            builder.Services.AddHttpClient<IRegisteredDogListService, RegisteredDogListService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

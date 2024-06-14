using System.Globalization;
using System.Reflection;
using Demo.Maui.Models;
using Demo.Maui.Services;
using Demo.Maui.Services.Common;
using Demo.Maui.Services.Interfaces;
using Demo.Maui.Utilities.Constants;
using Demo.Maui.Repositories.Interfaces;
using Demo.Maui.ViewModels;
using Demo.Maui.Views;
using Microsoft.Extensions.Configuration;
using Demo.Maui.Repositories;

namespace Demo.Maui.Extensions
{
    public static class MauiAppBuilderExtension
    {
        public static MauiAppBuilder ConfigureCulture(this MauiAppBuilder builder)
        {
            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            return builder;
        }

        public static MauiAppBuilder ConfigureViews(this MauiAppBuilder builder)
        {
            // Configure views
            //
            // Ex. builder.Services.AddSingleton<SplashPage>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AboutFilmPageView>();

            return builder;
        }

        public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
        {
            // Configure view models
            //
            // Ex. builder.Services.AddTransient<PreviewPhotoViewModel>();

            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AboutFilmPageViewModel>();

            return builder;
        }

        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            // Configure Maui Essentials

            builder.Services.AddSingleton(AppInfo.Current);
            builder.Services.AddSingleton(DeviceInfo.Current);
            builder.Services.AddSingleton(Connectivity.Current);
            builder.Services.AddSingleton(VersionTracking.Default);
            builder.Services.AddSingleton(Browser.Default);
            builder.Services.AddSingleton(DeviceDisplay.Current);
            builder.Services.AddSingleton(MediaPicker.Default);
            builder.Services.AddSingleton(FilePicker.Default);
            builder.Services.AddSingleton(Launcher.Default);
            builder.Services.AddSingleton(Geocoding.Default);
            builder.Services.AddSingleton(Geolocation.Default);
            builder.Services.AddSingleton(PhoneDialer.Default);
            builder.Services.AddSingleton(Email.Default);
            builder.Services.AddSingleton(Map.Default);
            builder.Services.AddSingleton(Share.Default);

            // Configure services
            //
            // Ex. builder.Services.AddSingleton<IApiService, ApiService>();

            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddSingleton<IMovieService, MovieService>();

            return builder;
        }

        public static MauiAppBuilder ConfigureRepositories(this MauiAppBuilder builder)
        {
            // Configure repositories
            //
            // Ex. builder.Services.AddSingleton<ISXCRepository, SXCRepository>();

            builder.Services.AddSingleton<ICacheRepository, CacheRepository>();

            return builder;
        }

        public static MauiAppBuilder ConfigureAppSettings(this MauiAppBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(AppConstants.AppSettingsPath);

            builder.Configuration.AddConfiguration(new ConfigurationBuilder().AddJsonStream(stream).Build());

            builder.Services.AddSingleton(builder.Configuration.GetRequiredSection(nameof(AppConfig)).Get<AppConfig>());

            return builder;
        }
    }
}


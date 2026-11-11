using System.Reflection;
using CourseCrutch.Clients.Models;
using CourseCrutch.Clients.Services;
using CourseCrutch.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using TesseractOcrMaui;

namespace CourseCrutch.Clients
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            using var settings = Assembly.GetExecutingAssembly().GetManifestResourceStream("appsettings.json");
            if (settings is not null)
            {
                builder.Configuration.AddJsonStream(settings);
            }

            builder.Logging.AddFileLogging(builder.Configuration, FileSystem.AppDataDirectory);

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddTesseractOcr(files => files.AddFile("eng.traineddata"));

            builder.Services.AddSingleton<IPhotoCaptureService, PhotoCaptureService>();
            builder.Services.AddSingleton<IOcrService, OcrService>();
            builder.Services.AddHttpClient<IOcrApiClient, OcrApiClient>(client =>
            {
                client.BaseAddress = new Uri(GetApiBaseAddress());
            });

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static string GetApiBaseAddress() =>
            DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5059/"
                : "http://localhost:5059/";
    }
}

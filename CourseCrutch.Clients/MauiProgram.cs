using CourseCrutch.Clients.Models;
using CourseCrutch.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;
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

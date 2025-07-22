using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PriceTracker.Components.Pages.States;
using PriceTracker.Data;
using PriceTracker.Data.Repositories;
using PriceTracker.Interfaces;
using ZXing.Net.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Services;
using CurrieTechnologies.Razor.SweetAlert2;

namespace PriceTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                            .UseMauiApp<App>()
                                        .UseBarcodeReader() // necessário para ZXing.Net.MAUI

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
            builder.Services.AddSingleton(new AppDbContext(dbPath));
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
            builder.Services.AddScoped<IProductVariationRepository, ProductVariationRepository>();
            builder.Services.AddHttpClient<GtinService>();

            builder.Services.AddSingleton<BarcodeState>();
            builder.Services.AddSingleton<AppSharedState>();
            builder.Services.AddSingleton<KeyboardStateService>();

            builder.Services.AddSweetAlert2();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddMudServices(); // Adicione esta linha

            return builder.Build();
        }
    }
}

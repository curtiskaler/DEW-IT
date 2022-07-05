using DewIt.Client.infrastructure;
using CommunityToolkit.Maui;
using MainPage = DewIt.Client.Features.MainWindow.MainPage;
using DewIt.Client.Features.MainWindow;
using DewIt.Model.Events;
using DewIt.Model.Persistence;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DewIt.Client;

public static class EntryPoint
{
	public static MauiApp CreateMauiApp()
	{
        System.Diagnostics.Debug.WriteLine("Entering EntryPoint!");

		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<DewItClient>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FontAwesome5Solid.otf", "FontAwesomeSolid");
            });

        // Add DI here
        builder.Services.AddSingleton<IResource, DBResource>();
        builder.Services.AddSingleton<ILaneRepository, LanesRepository>();
        builder.Services.AddSingleton<ICardRepository, CardsRepository>();
        builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        System.Diagnostics.Debug.WriteLine("Leaving EntryPoint!");
        return builder.Build();
	}
}

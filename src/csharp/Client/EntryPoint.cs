using DewIt.Client.infrastructure;
using CommunityToolkit.Maui;
using DewIt.Client.Dev;
using MainPage = DewIt.Client.Features.MainWindow.MainPage;
using DewIt.Client.Features.MainWindow;
using DewIt.Model.DataTypes;
using DewIt.Model.Events;
using DewIt.Model.Infrastructure;
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
			.RegisterFonts()
            .RegisterHandlers()
            .RegisterServices();
        
        System.Diagnostics.Debug.WriteLine("Leaving EntryPoint!");
        return builder.Build();
	}
}

internal static class DewItObjectGraphExtensions
{
    public static MauiAppBuilder RegisterFonts(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("FontAwesome5Solid.otf", "FontAwesomeSolid");
        });
        return builder;
    }
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        // TODO: Add a logger

        // TODO: Update to web resource 
        builder.Services.AddSingleton<IResource, DBResource>();

        builder.Services.AddSingleton<ILaneRepository, LanesRepository>();
        builder.Services.AddSingleton<ICardRepository, CardsRepository>();
        builder.Services.AddSingleton<IRepositoryCollection, RepositoryCollection>();
        
        builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        // TODO: Un-dev-ify the bootstrapper
        builder.Services.AddSingleton<IBootstrapper<DewItState>, DevBootstrapper>();
        builder.Services.AddSingleton<DewItState>();

        return builder;

        // Add your services here...
        // Default method
        //builder.Services.Add();

        // Scoped objects are the same within a request, but different across different requests.
        //builder.Services.AddScoped();     
        
        // Singleton objects are created as a single instance throughout the application. It creates the instance for the first time and reuses the same object in the all calls.
        //builder.Services.AddSingleton();  
        
        // Transient objects lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services.
        //builder.Services.AddTransient();  
    }
    public static MauiAppBuilder RegisterHandlers(this MauiAppBuilder builder)
    {
        //RegisterMappers();
        //return builder.ConfigureMauiHandlers(handlers =>
        //{
        //    // Your handlers here...
        //    //handlers.AddHandler(typeof(MyEntry), typeof(MyEntryHandler));
        //});
        return builder;
    }
}

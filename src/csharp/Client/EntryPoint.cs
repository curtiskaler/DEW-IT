using CommunityToolkit.Maui;
using DewIt.Client.infrastructure;
using DewIt.Client.Features.MainWindow;
using DewIt.Client.Infrastructure.Bootstrapping;
using DewIt.Model.DataTypes;
using DewIt.Model.Events;
using DewIt.Model.Infrastructure;
using DewIt.Model.Persistence;
using Microsoft.Extensions.Logging;
using MainPage = DewIt.Client.Features.MainWindow.MainPage;


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
            .RegisterLoggers()
            .RegisterFonts()
            .RegisterHandlers()
            .RegisterServices();

        System.Diagnostics.Debug.WriteLine("Object graph created. Leaving EntryPoint!");

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

    public static MauiAppBuilder RegisterLoggers(this MauiAppBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddDebug();

        return builder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        // dev resources : TODO: update to prod
        builder.Services.AddSingleton<IResource>(sp => new DBResource());
        builder.Services.AddSingleton<ILaneRepository>(sp =>
            new LanesRepository(
                sp.GetRequiredService<ILogger<LanesRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<ICardRepository>(sp =>
            new CardsRepository(
                sp.GetRequiredService<ILogger<CardsRepository>>(),
                sp.GetRequiredService<IResource>()));

        // prod resources
        builder.Services.AddSingleton<IRepositoryCollection>(sp =>
            new RepositoryCollection(
                sp.GetRequiredService<ILogger<RepositoryCollection>>(),
                sp.GetRequiredService<ILaneRepository>(),
                sp.GetRequiredService<ICardRepository>()));

        builder.Services.AddSingleton<IBootstrapData>(sp =>
            new BootstrapData(sp.GetRequiredService<IRepositoryCollection>()));

        builder.Services.AddSingleton<IBootstrapper<DewItState>>(sp =>
            new Bootstrapper(
                sp.GetRequiredService<ILogger<IBootstrapper<DewItState>>>(),
                sp.GetRequiredService<IBootstrapData>()));

        builder.Services.AddSingleton(sp =>
            new DewItState(sp.GetRequiredService<IBootstrapper<DewItState>>()));

        builder.Services.AddSingleton<IEventAggregator>(sp =>
            new EventAggregator());

        builder.Services.AddSingleton(sp =>
            new MainPageViewModel(
                sp.GetRequiredService<ILogger<MainPageViewModel>>(),
                sp.GetRequiredService<IRepositoryCollection>(),
                sp.GetRequiredService<IEventAggregator>()));

        builder.Services.AddSingleton(sp =>
            new MainPage(sp.GetRequiredService<MainPageViewModel>(),
                sp.GetRequiredService<IResource>()));

        // TODO: Un-dev-ify the bootstrapper


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
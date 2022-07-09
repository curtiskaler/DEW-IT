using CommunityToolkit.Maui;
using DewIt.Client.infrastructure;
using DewIt.Client.Features.MainWindow;
using DewIt.Client.Infrastructure.Bootstrapping;
using DewIt.Model.DataTypes;
using DewIt.Model.Events;
using DewIt.Model.Infrastructure;
using DewIt.Model.Persistence;
using DewIt.Model.Processing;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;
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

        // pre-build the app
        var app = builder.Build();

        // Intercept and register the ServiceProvider
        app.Services.UseResolver();

        // return the app as usual;
        return app;
    }
}

internal static class DewItObjectGraphExtensions
{
    public static MauiAppBuilder RegisterFonts(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(fonts =>
        {
            // ReSharper disable StringLiteralTypo
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("FontAwesome5Solid.otf", "FontAwesomeSolid");
            // ReSharper restore StringLiteralTypo
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
        // TODO: Un-dev-ify the bootstrapper
        // dev resources : TODO: update to prod
        builder.Services.AddSingleton<IResource>(new DBResource());
        builder.Services.AddSingleton<ILaneRepository>(sp =>
            new LanesRepository(
                sp.GetRequiredService<ILogger<LanesRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<ICardRepository>(sp =>
            new CardsRepository(
                sp.GetRequiredService<ILogger<CardsRepository>>(),
                sp.GetRequiredService<IResource>()));

        builder.Services.AddSingleton<IRepositoryCollection>(sp =>
            new RepositoryCollection(
                sp.GetRequiredService<ILogger<RepositoryCollection>>(),
                sp.GetRequiredService<ILaneRepository>(),
                sp.GetRequiredService<ICardRepository>()));


        builder.Services.AddSingleton<IBootstrapperServices>(sp =>
            new BootstrapperServices(sp.GetRequiredService<IRepositoryCollection>()));


        // prod resources
        builder.Services.AddSingleton<IProcessor>(
            sp => new Processor(sp.GetRequiredService<ILogger<IProcessor>>())
        );

        builder.Services.AddSingleton<IBootstrapper<DewItState>>(sp =>
            new Bootstrapper(
                sp.GetRequiredService<ILogger<IBootstrapper<DewItState>>>(),
                sp.GetRequiredService<IProcessor>(),
                sp.GetRequiredService<IBootstrapperServices>())
        );

        builder.Services.AddSingleton<IEventAggregator>(
            new EventAggregator()
        );

        builder.Services.AddSingleton(sp =>
            new MainPageViewModel(
                sp.GetRequiredService<ILogger<MainPageViewModel>>(),
                sp.GetRequiredService<IRepositoryCollection>(),
                sp.GetRequiredService<IEventAggregator>())
        );

        builder.Services.AddSingleton(sp =>
            new MainPage(sp.GetRequiredService<MainPageViewModel>(),
                sp.GetRequiredService<IResource>())
        );

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

[Obsolete("Don't use this. Bypassing DI is a code smell.")]
public static class Resolver
{
    /* Don't use this. Bypassing DI is a code smell. */
    
    /** How to use:
     *  At the bottom of CreateMauiApp(), do the following:
     * 
     *      // pre-build the app
     *      var app = builder.Build();
     * 
     *      // Intercept and register the ServiceProvider
     *      app.Services.UseResolver();
     *
     *      // return the app as usual;
     *      return app;
     */

    private static IServiceProvider _serviceProvider;

    public static IServiceProvider ServiceProvider =>
        _serviceProvider ?? throw new Exception("Service provider has not been initialized");

    /// <summary>
    /// Register the service provider
    /// </summary>
    public static void RegisterServiceProvider(IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the service provider.
    /// </summary>
    public static T Resolve<T>() where T : class
        => ServiceProvider.GetRequiredService<T>();

    public static void UseResolver(this IServiceProvider sp)
    {
        RegisterServiceProvider(sp);
    }
}
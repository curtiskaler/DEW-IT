using B7.Events;
using B7.Lifecycle;
using B7.Persistence;
using B7.Processing;
using DewIt.Client.Bootstrapping;
using DewIt.Client.Model;
using DewIt.Client.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DewIt.Client;

public static class EntryPoint
{
    public static MauiApp CreateMauiApp()
    {
        System.Diagnostics.Debug.WriteLine("EntryPoint CreateMauiApp!");
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<DewItClient>()
            .RegisterLoggers()
            .RegisterFonts()
            .RegisterHandlers()
            .RegisterServices();

        // pre-build the app
        var app = builder.Build();

        // Intercept and register the serviceProvider
        //app.Services.UseResolver();

        System.Diagnostics.Debug.WriteLine("Object graph created. Leaving EntryPoint!");

        // return the app as usual.
        return app;
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

    public static MauiAppBuilder RegisterHandlers(this MauiAppBuilder builder)
    {
        //RegisterMappers();
        //return builder.ConfigureMauiHandlers(handlers =>
        //{
        //    // Your handlers here...
        //    handlers.AddHandler(typeof(MyEntry), typeof(MyEntryHandler));
        //};
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
        // add your services here...
        // Default Method
        // builder.Services.Add();

        // Scoped objects are the same within a request, but different across
        // different requests.
        // builder.Services.AddScoped();

        // Singleton objects are created as a single instance throughout the
        // application. It creates the instance a single time, and reuses the
        // same object in all the calls.
        // builder.Services.AddSingleton();

        // Transient objects lifetime services are created each time they are
        // requested. This lifetime works best for lightweight, stateless
        // services.
        // builder.Services.AddTransient();

        builder.Services.AddSingleton<IResource>(new DBResource());
        builder.Services.AddSingleton<ILaneRepository>(sp =>
            new LaneRepository(
                sp.GetRequiredService<ILogger<LaneRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<ICardRepository>(sp =>
            new CardRepository(
                sp.GetRequiredService<ILogger<CardRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<IGroupRepository>(sp =>
            new GroupRepository(
                sp.GetRequiredService<ILogger<GroupRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<IUserRepository>(sp =>
            new UserRepository(
                sp.GetRequiredService<ILogger<UserRepository>>(),
                sp.GetRequiredService<IResource>()));
        builder.Services.AddSingleton<IRepositoryCollection>(sp =>
            new RepositoryCollection(
                sp.GetRequiredService<ILogger<RepositoryCollection>>(),
                sp.GetRequiredService<ILaneRepository>(),
                sp.GetRequiredService<ICardRepository>(),
                sp.GetRequiredService<IGroupRepository>(),
                sp.GetRequiredService<IUserRepository>()
            ));


        builder.Services.AddSingleton<IProcessor>(sp =>
            new Processor(sp.GetRequiredService<ILogger<IProcessor>>())
        );


        builder.Services.AddSingleton<IBootstrapperServices>(sp =>
            new BootstrapperServices(
                sp.GetRequiredService<IRepositoryCollection>()));
        builder.Services.AddSingleton<IBootstrapper<ClientState>>(sp =>
            new Bootstrapper(
                sp.GetRequiredService<ILogger<IBootstrapper<ClientState>>>(),
                sp.GetRequiredService<IProcessor>(),
                sp.GetRequiredService<IBootstrapperServices>()
        ));


        builder.Services.AddSingleton<IEventAggregator>(
            new EventAggregator()
        );




        return builder;
    }
}
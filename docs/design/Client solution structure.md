# Client Solution Structure #
This will be an evolving document. As things make sense, things will change.
For example, when I realized that plugins might be desirable, it made sense to 
split the truly cross-cutting concerns into a separate library (Model.csproj),
so that plugins could access them without creating circular references.

## Model.csproj ##
The shared model: cross-cutting concerns. Domain-specific items should be segregated.
This is stuff that PLUGINS might want to use.
```
Model   
 │  "IStartupService" / "IShutdownService"
 │  IBootstrapper
 ┊
 ┊  (cross-cutting concerns below:)
 ┊
 ├─── Algorithms          << (code that DOES stuff; "services")
 │                           [DataControllers, DataProviders, Extensions, Queries]
 │       StringFormatter
 │       ...etc.
 │
 ├─── DataTypes           << (code that DOES NOT do stuff; DTOs)
 │                           [Interfaces, Collections, Enums] 
 │       IProject
 │       IBoard
 │       ICard...
 ┊
 ┊  (Feature-specific goodies; service and view DEFINITIONS below:)
 ┊
 ├─── Plugins      << (How to import functionality into the app)
 │       IPlugin
 │       
 ├─── NavBar...
 ├─── Configuration...
 ├─── Persistence...
 ├─── Settings...
 ├─── Themes...
 ├─── Logging...
 ├─── Filtering...
 ├─── Searching...
 ├─── Notifications...
 ├─── I18n...
 ┊
```

## Client.csproj ##
The main client view.

```
Client
 │  EntryPoint   << (generate the entire object graph as close to here as possible.)
 │
 ├─── Model      << (the SHARED "model": stuff that cannot be restricted to a single feature)
 │      ├─── Algorithms   << (code that DOES stuff; "services")
 │      │                    [DataControllers, DataProviders, Extensions, Queries]
 │      │
 │      └─── DataTypes    << (code that DOES NOT do stuff; DTOs)
 │                           [Interfaces, Collections, Enums] 
 │               IProject
 │               IBoard
 │               ICard...
 │
 ├─── Dev   << (to be deleted before release, e.g., for bootstrapping w/o a backend)
 │       DevRepository
 │       
 ├─── Infrastructure      << (cross-cutting concerns ONLY; NO domain-specific objects or logic) 
 │      │      (ALL services are abstractions) 
 │      │      (no custom views here: they all take "plugins")
 │      │   App.xaml
 │      │   App.xaml                
 │      │   "StartupService" / "ShutdownService"
 │      │
 │      ├─── Resources
 │      │      ├─── Images
 │      │      ├─── Fonts... etc.
 │      │      
 │      ├─── MainWindow   << (The application window)
 │      │       IMainWindowViewModel
 │      │       MainWindowView
 │      │      
 │      ├─── Settings
 │      ┊       ISettingsHubViewModel
 │      ┊       SettingsHubView
 │                    
 └─── Features    (domain features, separated by view or what a user would call it)
        │      
        ├─── MainWindow
        │       MainWindowViewModel
        │       Bootstrapper
        │       
        ├─── NavBar...
        │       
        ├─── Splash
        │       SplashPageView
        │       
        ├─── HomePage
        │       HomePageViewModel
        │       HomePageView
        │                   
        ├─── ProjectView
        │       ProjectViewModel
        │       ProjectView
        │       
        │       ProjectFilter
        │       ProjectSearch
        │       ProjectRepository
        │                   
        ├─── BoardView
        │       BoardViewModel
        │       BoardView
        │       
        │       BoardFilter
        │       BoardSearch
        │       BoardRepository
        │                   
        ├─── LaneView...
        ├─── GroupView...
        ├─── CardView...
        ┊
        ┊                    (etc...)
        ┊
        └─── Settings     << (each page should be loaded like a plugin)        
               │ SettingsHubViewModel
               │ 
               ├─── Themes
               │        ThemeSettingsViewModel
               │        ThemeSettingsView
               │        "ThemesService"
               │      
               ├─── UserAccount 
               ┊        UserAccountSettingsViewModel
               ┊        UserAccountSettingsView
               ┊        "UserAccountService"
               ┊
               ┊   (etc...)
               ┊
```




## References ##

[Maybe it's time to rethink our project structure with .NET 6](https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6#a-domain-driven-api)

[MVVM best practices: folders and namespaces](https://social.msdn.microsoft.com/Forums/vstudio/en-US/300cde7a-a853-4d5d-b1a1-d92a246a60e0/mvvm-best-practices-folders-and-namespaces?forum=wpf)

[Project structure for MVVM in WPF](https://stackoverflow.com/questions/18825888/project-structure-for-mvvm-in-wpf)

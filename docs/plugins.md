# PLUGINS #

## Priority ##
Plugins are a fairly mature idea, but also a fairly deep and possibly thorny subject. 

They may become an important part of version "1", but...

<h1>PLUGINS ARE NOT CRITICAL FOR MVP</h1>

## Introduction ##

This will be my naive attempt at designing and implementing a plugin architecture. 

:) BuCkLe uP!

## First draft ##

Where to start? One [stackoverflow answer](https://stackoverflow.com/a/2826770) that I read suggested:

>An `IPlugin` interface is used to identify classes that implement plugins. Methods are added to the interface to allow the application to communicate with the plugin. For example the `Init` method that the application will use to instruct the plugin to initialize.
> 
> To find plugins the application scans a plugin folder for .Net assemblies. Each assembly is loaded. Reflection is used to scan for classes that implement `IPlugin`. An instance of each plugin class is created.
> 
> (Alternatively, an Xml file might list the assemblies and classes to load. This might help performance but I never found an issue with performance).
> 
> The `Init` method is called for each plugin object. It is passed a reference to an object that implements the application interface: `IApplication` (or something else named specific to your app, eg ITextEditorApplication).
> 
> `IApplication` contains methods that allows the plugin to communicate with the application. For instance if you are writing a text editor this interface would have an `OpenDocuments` property that allows plugins to enumerate the collection of currently open documents.
> 
> This plugin system can be extended to scripting languages, eg Lua, by creating a derived plugin class, eg `LuaPlugin` that forwards `IPlugin` functions and the application interface to a Lua script.
> 
> This technique allows you to iteratively implement your `IPlugin`, `IApplication` and other application-specific interfaces during development. When the application is complete and nicely refactored you can document your exposed interfaces and you should have a nice system for which users can write their own plugins.

Well, this sounds like a good start...

Naive also means 'marked by unaffected simplicity', which doesn't sound that bad.  Lets not do any premature optimization or refactoring.

## Types of Plugins ##
To get an idea of what we might want on the plugin interface, it would be handy to have an idea of what functionality we might want to extend.
Here are some things that I (or a customer) might want to extend or change:

1. Change the appearance of the MainView 
1. Change the color scheme of the application
1. Add a new bit of functionality to the menu
1. Add a new type of filtering 
1. Add a new type of searching
1. Import data from some arbitrary system
1. Export data to some arbitrary target
1. Add a new Settings page
1. Add a new view into the data
1. ...?  Profit!

## Considerations ##
- Plugins may need to depend on other plugins
- Plugins should not have access to data APIs
    - for security reasons, limit what the plugins have access to
- Plugins SHOULD have access to any data the UI has access to
- Plugins need to communicate (both ways) with the application
- Plugins will need access to UI-enhancing APIs
- either:
    - Plugins do not get special access to the backend
      
      ~ OR
    - Plugins need an authentication scheme, so that the client can make proxy calls on their behalf
- permissions:
    - Plugins are granted the same permissions as the current user
    - Plugins that need "service-account" must request enhanced permissions
    - Enhanced permissions must be allowable, storable, and revokable
- error handling
- 

## Interfaces ##

### Identifier ###
The `Identifier` is the UUID of your versioned plugin, such that a "plugin store" can uniquely identify it.
``` 
Identifier: {
    id: string              ex: 'com.one-two-three-GO.DEW-IT.demo"
    name: string            ex: "DEW IT Processing Demo"
    version: string         ex: "10.0.0"
}
```

### PackageInfo ###
The `PackageInfo` is the "package.json" of your plugin, and will be used by the "plugin store" to display information about your plugin.
```
PackageInfo : Identifier & {
    author: Contact             = { 
                                      name: string    = "", 
                                      email: string   = "", 
                                      url: string     = ""
                                  }
    contributors: Contact[]     = []
    description: string         = ""
    displayname: string         = ""
    funding: TypedURL           = {
                                      type: string = ""
                                      url: string = ""
                                  }
    homepage: string            = ""
    keywords: string[]          = []
    bugs: string                = ""
    license: string             = "UNLICENSED"
    private: boolean            = false
}
```

### IExtensionPoint &LT;INCOMPLETE&GT; ###
```
IExtensionPoint: Identifier & {
    ??? Whatever parts of your plugin you want to expose ???
}
```



### IPlugin &LT;INCOMPLETE&GT; ###
```
IPlugin: {
    metadata: PackageInfo
    dependencies: Identifier[]          = []
    extensionPoints: IExtensionPoint[]  = []

    <INCOMPLETE>
}

```

## Inter-Process Communication (IPC) for Plugins ##

Plugins should not have direct/root access to server data.
As such, there needs to be a 'sanitized' interface for requesting data or submitting changes.

### PluginIPC &LT;INCOMPLETE&GT; ###
Everything from the plugin should be assumed asynchronous.
Every call may be a server call, so no response may permanently block the UI thread.

```
PluginIPC: {
    
    // invokes the action on the given channel, using the specified args
    invoke<TArgs> ( channel: string, args: TArgs ): void
    
    // handles messages coming from the given channel
    handle<TArgs> ( channel: string, handler: Action<TArgs> ): void
}
```

#### Channels ####
Channels should be registered, and identifiable via
`<namespace>:<channel-name>`, where 'namespaces' can be also be prefixed in a readable way.

Some example namespaces:
- `ui`
  - `ui:dialog`
  - `ui:views`
    - `ui:views:<view-id>`
  - `ui:menu`
    - `ui:menu:<menu-id>`
  - `ui:filters`
  - `ui:search`
  - `ui:settings`
    - `ui:settings:<settings-page-id>`
- `data` (?? SECURITYYYyyy!!! ??)


Some example channels:
- `ui:dialog:openFile`
- `ui:dialog:saveFile`
- `data:cards:create`
- `data:cards:read`
- `data:cards:update`
- `data:cards:delete`

There should be no crashing when such a channel does not exist.

## References ##


[Notes on the Eclipse Plug-in Architecture](http://www.eclipse.org/articles/Article-Plug-in-architecture/plugin_architecture.html)

stackoverflow:
- [How To Create a Flexible Plug-In Architecture?](https://stackoverflow.com/a/2826770)
- [What are the advantages and disadvantages of plug-in based architecture?](https://stackoverflow.com/a/2851880)

Electron:
- [Inter-Process Communication](https://www.electronjs.org/docs/latest/tutorial/ipc)
- [Context Isolation](https://www.electronjs.org/docs/latest/tutorial/context-isolation)
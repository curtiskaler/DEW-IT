# Multi-language project structure #


## Lessons ##

  - We need a place for dev scripts
  - we need a place for build scripts
  - we need a place for config files
  - we need a place for CI ... config and jobs and scripts
  - we want a clear/discoverable place for dependency licenses
  - we want a clear/discoverable place for design docs
  - we need a place for compiled output 
  - having multiple packages in the root tends to clutter things

## Chosen Convention ## 

- Do NOT modify the .gitignore
- Keep your 'personal' folder under the `local` folder


legend:

| convention | meaning |
| --- | --- |
| **[thinger]** | git-ignored 'thinger' folder |
| **...** | et cetera |


```
Project
 │   .editorConfig  << NOT ignored, to keeping style consistent
 │   .gitignore
 │   README.md
 │
 ├── .vscode      << likely unignored for more specific style rules
 ├── .idea        << likely unignored for more specific style rules
 │
 ├── build        << build-system specific config files or scripts 
 │    ├── gradle
 │    ├── (other special conf folders)
 │    ┊
 │
 ├── docs
 │    ├── developer
 │    │     CODE_OF_CONDUCT.md
 │    │     CONTRIBUTING.md
 │    │     DEVELOPER.md
 │    │     *<etc>.md
 │    │   
 │    ├── design
 │    │     cool-idea.md
 │    │     *<etc>.*
 │    │     
 │    ├── licenses	<< (license files for dependencies)
 │    ┊     *<licenses>.*
 │      
 ├── local		  << LOCAL (developer-machine-specific) config files. 
 │    │   <special template files>.*
 │    │
 │    ├── CI            << resources specific to CI servers
 │    ├── gitlab        << resources specific to gitlab
 │    ┊    (etc.)
 │    ┊
 │    ┊         [remaining folders are 'personal', and .git-ignored]
 │    ├── <hostname1> 	<< resources specific to 'hostname1' 
 │    ├── <hostname2> 	<< resources specific to 'hostname2' 
 │    ┊ 
 │    
 ├── [output]
 │     ├── dev          << intermediary build folder
 │     ├── prod         << final release-mode built objects, license files, readme, docs, etc.
 │     └── release      << installers / deployment packages
 │   
 ├── resources	        << (non-code things to be packaged)
 │          			    [docs, config file templates, etc.]
 │      
 ├── src
 │    ├── csharp
 │    │    │
 │    │    │   <Project>.sln
 │    │    │   <Project>.sln.DotSettings
 │    │    │
 │    │    ├── client          << package 1
 │    │    │     *client.csproj
 │    │    │
 │    │    ├── model           << package 2
 │    │    ┊     *model.csproj
 │    │ 
 │    └── groovy
 │         ├── server
 │         ┊    └── com
 │         ┊         └── example
 │         ┊              └── project
 │         ┊                   ├── functional area 1
 │         ┊                   ...
 └── test
      ├── csharp
      │    ├── client.tests    << package 1
      │    │
      │    ├── model.tests     << package 2
      │    ┊
      │    
      └── groovy
           ├── server 
           ┊    ├── functional
           ┊    │    └── com
           ┊    │         └── example
           ┊    │              └── project
           ┊    │                   ├── functional area 1
           ┊    │                   ...
           ┊    ├── integration
           ┊    │    └── com
           ┊    │         └── example
           ┊    │              └── project
           ┊    │                   ├── functional area 1
           ┊    │                   ...
           ┊    └── unit
           ┊         └── com
           ┊              └── example
           ┊                   └── project
           ┊                        ├── functional area 1
           ┊                        ...
```


## Previous Examples ##

I've worked on a groovy project like this:

```
 Project
   ansible          << license and ansible yml files
   ci               << jenkins, gitlab, gradle config and jobs
   gradle           << app-repo and dependencies info
   shared           << shared build config for main and plugins
   (big thing)      << <package #1>
     .docker
     .gitlab-ci
     .idea
     .slcache
     .vscode
     CMRE
     conf
     grails-app     << (in-place install?)
     lib
     LinuxInstall
     out            << (compiled / inlined plugins?)
     README         
     scripts        << (dev tools)
     web-app        << (???)
     target         << (output folder)
     wrapper        << (gradle/grails wrapper)
     WI             << (web installer? i dunno)
     src
       groovy
         com
           org
             project
               ...
       javascript
         apps
         node_modules
         coverage
         scripts
     test
       functional
          com
            org
              project
                ...
       integration
          com
            org
              project
                ...
       unit
          com
            org
              project
   (tester)         << <package #2> (as package 1)
   (plugins)        << <package #3> (as package 1)
   (plugins-tester) << <package #4> (as package 1)
     etc.
     
     
                ...
```

...and a c# project like this:

```
 Project
   ci               << (jenkins / gitlab configs and jobs)
   CMRE             << (a tool used to build and validate the build)
   config files     << (shared config files... GlobalAssemblyInfo.cs / GlobalResources.resx)
   dev              << (build scripts folder)   
   license          << (license files for dependencies)
   packaging        << (how to build for various customers)
   redirects        << (how to configure for various customers)
   temp             << (junk folder)   
   output
     distribution   << (compiled files go here)
       
   (client)                 << <package 1>
     compatibility
        <solution 1>
        <solution 2>
        *<sln files>.sln
     client UI
        <solution 1>
        <solution 2>
        *<sln files>.sln
        FunctionalTests
            <solution 1>.FunctionalTests
            <solution 2>.FunctionalTests
        UnitTests
            <solution 1>.UnitTests
            <solution 2>.UnitTests
     client plugins
        <solution 1>
        <solution 2>
        *<sln files>.sln
   (server)                 << <package 2> (as package 1)
   ("framework" / shared)   << <package 3> (as package 1)
   (integration)            << <package 4> (as package 1)
   (help)                   << <package 5> (as package 1)
   (dev tool #1)            << <package 6> (as package 1)
    etc.
```

## Consider ##
- [EditorConfig](https://editorconfig.org/)
- [MS Docs / Visual Studio / IDE / Develop / Editor / Code style preferences](https://docs.microsoft.com/en-us/visualstudio/ide/code-styles-and-code-cleanup?view=vs-2022)


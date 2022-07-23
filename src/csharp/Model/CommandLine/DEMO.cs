namespace DewIt.Model.CommandLine;

public class DEMO
{
    /* Example CLI support for:
         *
         * Name
         *  dotnet add package - Adds or updates a package reference in a project file.
         *
         * Synopsis
         *
         *      dotnet add [<PROJECT>] package <PACKAGE_NAME>
         *          [-f|--framework <FRAMEWORK>] [--interactive]
         *          [-n|--no-restore] [--package-directory <PACKAGE_DIRECTORY>]
         *          [--prerelease] [-s|--source <SOURCE>] [-v|--version <VERSION>]
         *
         *      dotnet add package -h|--help
         *
         * Description
         *  The dotnet add package command provides a convenient option to add or
         *  update a package reference in a project file. When you run the command,
         *  there's a compatibility check to ensure the package is compatible with
         *  the frameworks in the project. If the check passes and the package isn't
         *  referenced in the project file, a <PackageReference> element is added to
         *  the project file. If the check passes and the package is already
         *  referenced in the project file, the <PackageReference> element is
         *  updated to the latest compatible version. After the project file is
         *  updated, dotnet restore is run.
         *
         * Arguments
         *  * PROJECT
         *      Specifies the project file. If not specified, the command searches
         *      the current directory for one.
         *
         *  * PACKAGE_NAME
         *      The package reference to add.
         *
         * Options
         *
         *  -f|--framework <FRAMEWORK>
         *      Adds a package reference only when targeting a specific framework.
         *
         *  -?|-h|--help
         *      Prints out a description of how to use the command.
         *
         *  --interactive
         *      Allows the command to stop and wait for user input or action. For example, to complete
         *      authentication.
         *
         *  -n|--no-restore
         *      Adds a package reference without performing a restore preview and compatibility check.
         *
         *  --package-directory <PACKAGE_DIRECTORY>
         *      The directory where to restore the packages. The default package restore location
         *      is %userprofile%\.nuget\packages on Windows and ~/.nuget/packages on macOS and Linux.
         *      For more information, see Managing the global packages, cache, and temp folders in NuGet.
         *
         *  --prerelease
         *      Allows prerelease packages to be installed. Available since .NET Core 5 SDK
         *
         *  -s|--source <SOURCE>
         *      The URI of the NuGet package source to use during the restore operation.
         *
         *  -v|--version <VERSION>
         *      Version of the package. See NuGet package versioning.
         */

    //public static void Start1()
    //{
        

    //    var parser = CommandLineParserBuilder.GetInstance()
    //        .SetDescription("dotnet command description")
            
    //        .AddOption("help","Prints out a description of how to use the command.","h", "?") // help for 'dotnet'

    //        .AddSubCommand("add").RequiresSubCommand()
    //        .AddOptionalArgument("project", "Specifies the project file. If not specified, the command searches the current directory for one.")
            
    //        .AddOption("help","Prints out a description of how to use the command.","h", "?") // help for 'dotnet add'
            
    //        .AddSubCommand("package")
    //        .SetDescription("The dotnet add package command provides a convenient option to add or " +
    //                        "update a package reference in a project file. When you run the command, " +
    //                        "there's a compatibility check to ensure the package is compatible with " +
    //                        "the frameworks in the project. If the check passes and the package isn't " +
    //                        "referenced in the project file, a <PackageReference> element is added to " +
    //                        "the project file. If the check passes and the package is already " +
    //                        "referenced in the project file, the <PackageReference> element is " +
    //                        "updated to the latest compatible version. After the project file is " +
    //                        "updated, dotnet restore is run.")
    //        .AddRequiredArgument("package_name", "The package reference to add.")

    //        .AddOption("help","Prints out a description of how to use the command.","h", "?") // help for 'dotnet add package'
            
    //        /*  -f|--framework <FRAMEWORK>
    //         *      Adds a package reference only when targeting a specific framework. */
    //        .AddOption("framework","Adds a package reference only when targeting a specific framework.", "f")
    //        .AddRequiredArgument("framework")
            
    //        /*  --interactive
    //         *      Allows the command to stop and wait for user input or action. For example, to complete
    //         *      authentication. */
    //        .AddOption("interactive", "Allows the command to stop and wait for user input or action. For example, to complete authentication.")
            
    //        /*  -n|--no-restore
    //         *      Adds a package reference without performing a restore preview and compatibility check.*/
    //        .AddOption("no-restore", "Adds a package reference without performing a restore preview and compatibility check.", "n")

    //        /*  --package-directory <PACKAGE_DIRECTORY>
    //         *      The directory where to restore the packages. The default package restore location
    //         *      is %userprofile%\.nuget\packages on Windows and ~/.nuget/packages on macOS and Linux.
    //         *      For more information, see Managing the global packages, cache, and temp folders in NuGet. */
    //        .AddOption("package-directory")
    //        .SetDescription("The directory where to restore the packages. The default package restore location "+
    //                        "is %userprofile%\\.nuget\\packages on Windows and ~/.nuget/packages on macOS and Linux. "+
    //                        "For more information, see Managing the global packages, cache, and temp folders in NuGet.")
    //        .AddRequiredArgument("package_directory")
            
    //        /*  --prerelease
    //        *      Allows prerelease packages to be installed. Available since .NET Core 5 SDK */
    //        .AddOption("prerelease", "Allows prerelease packages to be installed. Available since .NET Core 5 SDK.")

    //        /*  -s|--source <SOURCE>
    //         *      The URI of the NuGet package source to use during the restore operation. */
    //        .AddOption("source", "The URI of the NuGet package source to use during the restore operation.", "s")
    //        .AddRequiredArgument("source")

    //        /*  -v|--version <VERSION>
    //         *      Version of the package. See NuGet package versioning. */
    //        .AddOption("version", "Version of the package. See NuGet package versioning.", "v")
    //        .AddRequiredArgument("version")
            
    //        .Build();

    //    //var commandLine = new CommandLineBuilder("Pizza Ordering System")
    //    //    .AddOption("d", "debug", "output extra debugging")
    //    //    .AddOption("p", "pizza-type", "flavour of pizza")
    //    //    .WithArgument("type", "flavour of pizza")
    //    //    .AddOption("s", "small", "small pizza slice");

    //    //var parsedCommandLineOptions = commandLine.Parse();
    //}
}
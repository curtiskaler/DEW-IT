using DewIt.Model.CommandLine.Model;

namespace DewIt.Model.CommandLine;

/// <summary> An abstract builder interface. </summary>
/// <typeparam name="TProduct">The type of product that the builder will produce.</typeparam>
public interface IBuilder<out TProduct>
{
    /// <summary> Validates the specified configuration, and returns the corresponding <typeparamref name="TProduct"/> (or throws an error if the configuration is wrong). </summary>
    TProduct Build();
}

public interface IDescriptionBuilder<out TProduct, out TBuilder>
    where TBuilder : IDescriptionBuilder<TProduct, TBuilder>
{
    /// <summary> Sets the description (of the <typeparamref name="TProduct"/>) to be displayed in the command-line program help. </summary>
    TBuilder SetDescription(string description);
}

public interface ICLIEntryBuilder<out TProduct, out TBuilder> : IDescriptionBuilder<TProduct, TBuilder>,
    IBuilder<ICLIParser>
    where TBuilder : ICLIEntryBuilder<TProduct, TBuilder>
{
    TBuilder AddOptionalArgument(string name);
    TBuilder AddOptionalArgument(string name, string shortDescription);

    TBuilder AddRequiredArgument(string name);
    TBuilder AddRequiredArgument(string name, string shortDescription);

    ICLIOptionBuilder AddOption(string name);
    ICLIOptionBuilder AddOption(string name, string shortDescription, params string[] aliases);

    ICLICommandBuilder AddSubCommand(string name);
    ICLICommandBuilder AddSubCommand(string name, string shortDescription);

    ICLICommandBuilder AddSiblingCommand(string name);
    ICLICommandBuilder AddSiblingCommand(string name, string shortDescription);

    /* this won't work for 
        dotnet
            add
                package
                pepperoni
            remove
                package
                pepperoni
            create
                package
                solution
            etc.
      */      
}

public interface ICLIOptionBuilder : ICLIEntryBuilder<CLIOption, ICLIOptionBuilder>
{
}

public interface ICLICommandBuilder : ICLIEntryBuilder<CLICommand, ICLICommandBuilder>
{
    ICLICommandBuilder RequiresSubCommand();
}

public interface ICLIParserBuilder : ICLICommandBuilder
{
    ICLIParserBuilder SetArgumentDelimiters(List<string> list);

    ICLIParserBuilder SetFlagDelimiters(List<string> list);
}

public abstract class CommandLineParserBuilder : ICLIParserBuilder
{
    protected List<string> ArgumentDelimiters = new() { " ", ":", "=" };

    /** TODO: be careful with forward-slash, because on posix systems - 
     * basically everything but Windows - this is used as a PATH
     * delimiter, and must not confuse the parser. */
    protected List<string> FlagDelimiters = new() { "/", "-", "--" };


    /// <summary> Instantiates a new instance of the <see cref="CommandLineParserBuilder"/> class. </summary>
    public static ICLIParserBuilder GetInstance()
    {
        return new CLIBuilderImplementation();
    }
    
    public abstract ICLIParserBuilder SetArgumentDelimiters(List<string> list);
    public abstract ICLIParserBuilder SetFlagDelimiters(List<string> list);

    public abstract ICLIParser Build();


    private class CLIBuilderImplementation : CommandLineParserBuilder
    {
        public override ICLICommandBuilder RequiresSubCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary> Validates the specified configuration, and returns the corresponding <see cref="CommandLineParser"/> (or throws an error if the configuration is wrong). </summary>
        public override CommandLineParser Build()
        {
            Validate(this);
            return new CommandLineParser(AppDescription!);
        }

        /// <summary> Sets the desired description to be displayed in the command-line program help. </summary>
        public override CommandLineParserBuilder SetDescription(string description)
        {
            AppDescription = description;
            return this;
        }

        public override CommandLineParserBuilder SetArgumentDelimiters(List<string> list)
        {
            ArgumentDelimiters = list;
            return this;
        }

        public override CommandLineParserBuilder SetFlagDelimiters(List<string> list)
        {
            FlagDelimiters = list;
            return this;
        }

        public override ICLICommandBuilder AddSubCommand(string name)
        {
            throw new NotImplementedException();
        }

        private static void Validate(CommandLineParserBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (string.IsNullOrWhiteSpace(builder.AppDescription))
                throw new ArgumentException("AppDescription must not be null or whitespace.");
        }
    }
}

//public static class CommandLineBuilderExtensions
//{
//    // you don't add sub-commands to an option
//    public static ICLICommandBuilder AddSubCommand(this ICLIOptionBuilder optionBuilder, string name)
//    {
//        // find the parent
//        var that = default(ICLICommandBuilder);
//        return that!;
//    }
//    public static ICLICommandBuilder AddSubCommand(this ICLIOptionBuilder optionBuilder, string name, string shortDescription)
//    {
//        // find the parent
//        var that = default(ICLICommandBuilder);
//        return that!;
//    }
//}

//public static class CLICommandExtensions
//{
//    public static CLICommand SetDescription(this CLICommand command, string description)
//    {
//        command.Description = description;
//        return command;
//    }

//    public static CLICommand AddOption(this CLICommand command, string optionName)
//    {
//        return AddOption(command, optionName, "");
//    }

//    public static CLICommand AddOption(this CLICommand command, string optionName, string description,
//        params string[] aliases)
//    {
//        // TODO: Implement
//        return command;
//    }

//    public static CLICommand AddSubCommand(this CLICommand command, string cmdName)
//    {
//        return AddSubCommand(command, cmdName, "");
//    }

//    public static CLICommand AddSubCommand(this CLICommand command, string cmdName, string description,
//        params string[] aliases)
//    {
//        // TODO: Implement
//        return command;
//    }

//    public static CLICommand RequiresSubCommand(this CLICommand command)
//    {
//        // TODO: Implement
//        return command;
//    }

//    public static CLICommand AddOptionalArgument(this CLICommand command, string name, string shortDescription)
//    {
//        // TODO: Implement
//        return command;
//    }

//    public static CLICommand AddRequiredArgument(this CLICommand command, string name)
//    {
//        return AddRequiredArgument(command, name, "");
//    }

//    public static CLICommand AddRequiredArgument(this CLICommand command, string name, string shortDescription)
//    {
//        // TODO: Implement
//        return command;
//    }
//}
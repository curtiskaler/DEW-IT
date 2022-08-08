namespace DewIt.Model.CommandLine;

public interface ICLIParser
{
    ParsedCommandLine Parse();
    ParsedCommandLine Parse(string[] args);
}

public class CommandLineParser : ICLIParser
{
    public Command RootCommand { get; }

    public CommandLineParser(string appDescription)
    {
        RootCommand = new Command("Application Root Command", appDescription);
    }

    public ParsedCommandLine Parse()
    {
        return Parse(Environment.GetCommandLineArgs());
    }

    public ParsedCommandLine Parse(string[] args)
    {
        var result = new ParsedCommandLine(args);
        


        return result;
    }






    #region Builder Methods

    public CommandLineParser WithArgument()
    {
        return WithArgument("", "", false, Arity.ONE);
    }
    
    public CommandLineParser WithArgument(string longName, string description)
    {
        return WithArgument(longName, description, false, Arity.ONE);
    }

    public CommandLineParser WithArgument(string longName, string description, bool isRequired)
    {
        return WithArgument(longName, description, isRequired, Arity.ONE);
    }

    public CommandLineParser WithArgument(string longName, string description, bool isRequired, Arity arity)
    {
        return WithArgument(new Argument(longName, description, isRequired, arity));
    }

    public CommandLineParser WithArgument(Argument argument)
    {
        RootCommand.Arguments.Add(argument);
        return this;
    }

    public void Add(Argument argument)
    {
        RootCommand.Arguments.Add(argument);
    }
    




    public CommandLineParser WithCommand()
    {
        return WithCommand("", "", new string[] { });
    }

    public CommandLineParser WithCommand(string token, string description)
    {
        return WithCommand(token, description, new string[] { });
    }

    public CommandLineParser WithCommand(string token, string description, params string[] aliases)
    {
        return WithCommand(token, description, false, aliases);
    }

    public CommandLineParser WithCommand(string token, string description, bool requireSubCommand, params string[] aliases)
    {
        return WithCommand(new Command(token, description, aliases){RequiresSubCommand = requireSubCommand});
    }

    public CommandLineParser WithCommand(Command command)
    {
        RootCommand.SubCommands.Add(command);
        return this;
    }

    public Command AddCommand()
    {
        return AddCommand("", "", new string[] { });
    }

    public Command AddCommand(string token, string description)
    {
        return AddCommand(token, description, new string[] { });
    }

    public Command AddCommand(string token, string description, params string[] aliases)
    {
        return AddCommand(token, description, false, aliases);
    }

    public Command AddCommand(string token, string description, bool requireSubCommand, params string[] aliases)
    {
        return AddCommand(new Command(token, description, aliases) { RequiresSubCommand = requireSubCommand });
    }

    public Command AddCommand(Command command)
    {
        RootCommand.SubCommands.Add(command);
        return command;
    }
    
    public void Add(Command command)
    {
        RootCommand.SubCommands.Add(command);
    }
    
    public CommandLineParser WithFlag()
    {
        return WithFlag("", "", new string[] { });
    }

    public CommandLineParser WithFlag(string longName, string description)
    {
        return WithFlag(longName, description, new string[] { });
    }

    public CommandLineParser WithFlag(string longName, string description, params string[] aliases)
    {
        return WithFlag(new Flag(longName, description, aliases));
    }

    public CommandLineParser WithFlag(Flag flag)
    {
        RootCommand.Flags.Add(flag);
        return this;
    }

    public void Add(Flag flag)
    {
        RootCommand.Flags.Add(flag);
    }
    #endregion
}
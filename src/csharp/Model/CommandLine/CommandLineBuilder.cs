namespace DewIt.Model.CommandLine;

public class CommandLineBuilder
{
    public CommandLineBuilder(string appDescription)
    {
        RootCommand = new CLICommand("Application Root Command", appDescription);
    }

    private CLICommand RootCommand { get; }

    public CommandLineBuilder AddOption(string alias, string token, string description)
    {
        // an option might apply only to a specific command,
        // so we need to be specific

        // take the last added command, and apply the option
        // if no commands exist, then use the root command
        var target = RootCommand.SubCommands.LastOrDefault() ?? RootCommand;
        var newOption = new CLIOption(token, alias, description);
        target.Options.Add(newOption);
        RootCommand.LastModifiedOption = newOption;
        return this;
    }

    public CommandLineBuilder WithArgument(string argToken, string description)
    {
        // take the last added option (if any), and apply an argument
        var targetOpt = RootCommand.LastModifiedOption;
        if (targetOpt == null)
        {
            var targetCmd = RootCommand.LastModifiedCommand ?? RootCommand;
        }
        // if no options exist, then the last command
        // if no commands exist, then use the root command
        return this;
    }

    public ParsedCommandLine Parse()
    {
        return Parse(Environment.GetCommandLineArgs());
    }

    public ParsedCommandLine Parse(string[] args)
    {
        var result = new ParsedCommandLine();
        result.Tokens.AddRange(args);


        return result;
    }
}
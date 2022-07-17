namespace DewIt.Model.CommandLine;

public abstract class CLIEntry
{
    public string Token { get; set; }
    public string? Alias { get; set; }
    public string Description { get; set; }
    public int NumberOfPossibleArguments { get; set; }
    public int NumberOfRequiredArguments { get; set; }

    protected CLIEntry(string token, string description) : this(token, null, description, 0, 0)
    {
    }

    protected CLIEntry(string token, string alias, string description) : this(token, alias, description, 0, 0)
    {
    }

    protected CLIEntry(string token, string? alias, string description, int numberOfPossibleArguments,
        int numberOfRequiredArguments)
    {
        Token = token;
        Alias = alias;
        Description = description;
        NumberOfPossibleArguments = numberOfPossibleArguments;
        NumberOfRequiredArguments = numberOfRequiredArguments;
    }
}

/// <summary> Cross-cutting commands that apply to ALL applications. </summary>
/// Like redirect to stderr or stdout
public class CLIDirective : CLICommand
{
    public CLIDirective(string token, string description) : base(token, description)
    {
    }

    public CLIDirective(string token, string alias, string description) : base(token, alias, description, 0, 0)
    {
    }

    public CLIDirective(string token, string? alias, string description, int numberOfPossibleArguments, int numberOfRequiredArguments) : base(token, alias, description, numberOfPossibleArguments, numberOfRequiredArguments)
    {
    }
}

/// <summary> Modify the behavior of a command. </summary>
public class CLIOption : CLIEntry
{
    public CLIOption(string token, string description) : base(token, description)
    {
    }

    public CLIOption(string token, string alias, string description) : base(token, alias, description, 0, 0)
    {
    }

    public CLIOption(string token, string? alias, string description, int numberOfPossibleArguments, int numberOfRequiredArguments) : base(token, alias, description, numberOfPossibleArguments, numberOfRequiredArguments)
    {
    }
}

/// <summary>
/// An action that is supported by the application command line.
/// </summary>
public class CLICommand : CLIEntry
{
    public List<CLICommand> SubCommands { get; }
    public List<CLIOption> Options { get; }

    internal CLIOption? LastModifiedOption { get; set; } = null;
    internal CLICommand? LastModifiedCommand { get; set; } = null;

    public CLICommand(string token, string description) : this(token, null, description,0,0)
    {
    }

    public CLICommand(string token, string alias, string description) : this(token, alias, description, 0, 0)
    {
    }

    public CLICommand(string token, string? alias, string description, int numberOfPossibleArguments, int numberOfRequiredArguments) : base(token, alias, description, numberOfPossibleArguments, numberOfRequiredArguments)
    {
        SubCommands = new List<CLICommand>();
        Options = new List<CLIOption>();
    }
}
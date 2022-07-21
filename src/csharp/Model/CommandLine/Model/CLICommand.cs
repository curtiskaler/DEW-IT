namespace DewIt.Model.CommandLine.Model;

/// <summary>
/// An action that is supported by the application command line.
/// </summary>
public class CLICommand : CLIEntry
{
    public List<CLICommand> SubCommands { get; }
    public List<CLIOption> Options { get; }

    public CLICommand() : this("", null, "")
    {
    }

    public CLICommand(string token, string description) : this(token, null, description)
    {
    }

    public CLICommand(string token, string? alias, string description) : base(token, alias, description)
    {
        SubCommands = new List<CLICommand>();
        Options = new List<CLIOption>();
    }
}


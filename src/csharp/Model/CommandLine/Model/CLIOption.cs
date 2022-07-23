namespace DewIt.Model.CommandLine.Model;

/// <summary> Modify the behavior of a command. </summary>
public class CLIOption : CLIEntry
{
    public CLIOption(string token, string description) : base(token, description)
    {
    }

    public CLIOption(string token, string alias, string description) : base(token, alias, description)
    {
    }
}
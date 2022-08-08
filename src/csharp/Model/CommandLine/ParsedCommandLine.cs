namespace DewIt.Model.CommandLine;

public class ParsedCommandLine //: IParsedCommandLine
{

    /// <summary> The original command-line arguments. </summary>
    public List<string> Args { get; set; }

    /// <summary> The commands used. </summary>
    public List<Command> Commands { get; set; } = new();

    /// <summary> The flags used. </summary>
    public List<Flag> Flags { get; set; } = new();

    public ParsedCommandLine(IEnumerable<string> args)
    {
        Args = args.ToList();
    }
}

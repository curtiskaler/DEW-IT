using DewIt.Model.CommandLine.Model;

namespace DewIt.Model.CommandLine;

public class ParsedCommandLine //: IParsedCommandLine
{
    public List<string> Tokens { get; set; } = new();
    public List<CLICommand> Commands { get; set; } = new();
}

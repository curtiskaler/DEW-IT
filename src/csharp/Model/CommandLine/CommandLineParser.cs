using DewIt.Model.CommandLine.Model;

namespace DewIt.Model.CommandLine;

public interface ICLIParser
{
    ParsedCommandLine Parse();
    ParsedCommandLine Parse(string[] args);
}

public class CommandLineParser : ICLIParser
{
    private CLICommand RootCommand { get; }

    public CommandLineParser(string appDescription)
    {
        RootCommand = new CLICommand("Application Root Command", appDescription);
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
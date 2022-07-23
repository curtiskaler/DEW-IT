namespace DewIt.Model.CommandLine;

public class CommandLineConfiguration
{
    protected List<string> ArgumentDelimiters = new() { " ", ":", "=" };

    /** TODO: be careful with forward-slash, because on posix systems - 
     * basically everything but Windows - this is used as a PATH
     * delimiter, and must not confuse the parser. */
    protected List<string> FlagDelimiters = new() { "/", "-", "--" };

    public bool ShowHelpWhenPassedNoOptions { get; set; } = false;
}
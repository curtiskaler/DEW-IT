namespace DewIt.Model.CommandLine;

public class CommandLineConfiguration
{
    /// <summary>
    /// When <see langword="true"/>, starting the application
    /// with no arguments will result in the help or man page
    /// being displayed, and the application will immediately exit.
    /// </summary>
    public bool RequireArguments { get; set; } = false;

    public List<char> ArgumentDelimiters = new() { ' ', ':', '=' };

    /** TODO: be careful with forward-slash, because on posix systems - 
     * basically everything but Windows - this is used as a PATH
     * delimiter, and must not confuse the parser. */
    public List<string> FlagDelimiters = new() { "/", "-", "--" };
}
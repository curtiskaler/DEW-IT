namespace DewIt.Model.CommandLine.Model;

/// <summary> Cross-cutting commands that apply to ALL applications. </summary>
/// Like redirect to stderr or stdout
public class CLIDirective : CLICommand
{
    public CLIDirective(string token, string description) : base(token, description)
    {
    }

    public CLIDirective(string token, string alias, string description) : base(token, alias, description)
    {
    }
}
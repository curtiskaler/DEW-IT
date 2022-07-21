namespace DewIt.Model.CommandLine.Model;

public abstract class CLIEntry
{
    public string Token { get; set; }
    public string? Alias { get; set; }
    public string Description { get; set; }
    
    protected CLIEntry(string token, string description) : this(token, null, description)
    {
    }

    protected CLIEntry(string token, string? alias, string description)
    {
        Token = token;
        Alias = alias;
        Description = description;
        
    }
}
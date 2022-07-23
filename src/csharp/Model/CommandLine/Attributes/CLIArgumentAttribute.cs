namespace DewIt.Model.CommandLine;

public class BaseCLIAttribute : Attribute
{
    public string Name { get; set; }
    public List<string> Aliases { get; set; } = new();
    public string Description { get; set; }

    public BaseCLIAttribute(string name) : this(name, "")
    {
    }

    public BaseCLIAttribute(string name, string description, params string[] aliases)
    {
        Name = name;
        Description = description ?? "";
        Aliases = aliases?.ToList() ?? new List<string>();
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CLIArgumentAttribute : BaseCLIAttribute
{
    public CLIArgumentAttribute(string name) : this(name, "")
    {
    }

    public CLIArgumentAttribute(string name, string description, params string[] aliases) : base(name, description,
        aliases)
    {
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CLIFlagAttribute : BaseCLIAttribute
{
    public CLIFlagAttribute(string name) : this(name, "")
    {
    }

    public CLIFlagAttribute(string name, string description, params string[] aliases) 
        : base(name, description, aliases)
    {
    }
}
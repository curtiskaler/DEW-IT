using System.Diagnostics;

namespace DewIt.Model.CommandLine;

/// <summary>
/// <i>Commands</i> are actions that are supported by the application command line.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Command: ArgumentHolder<Command>
{
    public string Description { get; set; }
    public List<Flag> Flags { get; }
    public string LongName { get; set; }
    public bool RequiresSubCommand { get; set; }
    public List<string> ShortAliases { get; set; }
    public List<Command> SubCommands { get; }

    public Command() : this("", "")
    {
    }

    public Command(string longName, string description) : this(longName, description, false)
    {
    }

    public Command(string longName, string description, bool requiresSubCommand) : this(longName, description,
        requiresSubCommand, new string[] { })
    {
    }

    public Command(string longName, string description, params string[] aliases) : this(longName, description,
        false, aliases)
    {
    }


    public Command(string longName, string description, bool requiresSubCommand, params string[] aliases)
    {
        Description = description;
        Flags = new List<Flag>();
        LongName = longName;
        RequiresSubCommand = requiresSubCommand;
        ShortAliases = aliases.ToList();
        SubCommands = new List<Command>();
    }
    
    public Command WithFlag()
    {
        return WithFlag("", "", new string[] { });
    }

    public Command WithFlag(string longName, string description)
    {
        return WithFlag(longName, description, new string[] { });
    }

    public Command WithFlag(string longName, string description, params string[] aliases)
    {
        return WithFlag(new Flag(longName, description, aliases));
    }

    public Command WithFlag(Flag flag)
    {
        Flags.Add(flag);
        return this;
    }

    public void Add(Flag flag)
    {
        Flags.Add(flag);
    }

    public Flag AddFlag()
    {
        return AddFlag("", "", new string[] { });
    }

    public Flag AddFlag(string longName, string description)
    {
        return AddFlag(longName, description, new string[] { });
    }

    public Flag AddFlag(string longName, string description, params string[] aliases)
    {
        return AddFlag(new Flag(longName, description, aliases));
    }

    public Flag AddFlag(Flag flag)
    {
        Flags.Add(flag);
        return flag;
    }

    public Command WithSubCommand(string token, string description)
    {
        return WithSubCommand(token, description, false, new string[] { });
    }

    public Command WithSubCommand(string token, string description, bool requireSubCommand)
    {
        return WithSubCommand(token, description, requireSubCommand, new string[] { });
    }

    public Command WithSubCommand(string token, string description, bool requireSubCommand,
        params string[] aliases)
    {
        return WithSubCommand(new Command(token, description, requireSubCommand, aliases));
    }

    public Command WithSubCommand(Command subCommand)
    {
        SubCommands.Add(subCommand);
        return this;
    }

    public void Add(Command subCommand)
    {
        SubCommands.Add(subCommand);
    }
    
    public Command AddSubCommand(string token, string description)
    {
        return AddSubCommand(token, description, false, new string[] { });
    }

    public Command AddSubCommand(string token, string description, bool requireSubCommand)
    {
        return AddSubCommand(token, description, requireSubCommand, new string[] { });
    }

    public Command AddSubCommand(string token, string description, bool requireSubCommand,
        params string[] aliases)
    {
        return AddSubCommand(new Command(token, description, requireSubCommand, aliases));
    }

    public Command AddSubCommand(Command subCommand)
    {
        SubCommands.Add(subCommand);
        return subCommand;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay =>
        $"{LongName} {ShortAliasesDisplay}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string ShortAliasesDisplay =>
        $"{(ShortAliases.Count != 0 ? "(" + string.Join(',', ShortAliases) + ")" : "")}";
}
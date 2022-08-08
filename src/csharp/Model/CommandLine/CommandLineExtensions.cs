namespace DewIt.Model.CommandLine;

public static class CommandLineExtensions
{
    
    public static void Add(this List<Argument> arguments)
    {
        Add(arguments, "", "", false, Arity.ONE);
    }

    public static void Add(this List<Argument> arguments, string token, string description)
    {
        Add(arguments, token, description, false, Arity.ONE);
    }

    public static void Add(this List<Argument> arguments, string token, string description, bool isRequired,
        Arity arity)
    {
        var newArgument = new Argument(token, description, isRequired, arity);
        arguments.Add(newArgument);
    }
    
    public static void Add(this List<Flag> flags)
    {
        Add(flags, "", "", new string[] { });
    }

    public static void Add(this List<Flag> flags, string token, string description)
    {
        Add(flags, token, description, new string[] { });
    }

    public static void Add(this List<Flag> flags, string token, string description, params string[] aliases)
    {
        var newFlag = new Flag(token, description, aliases);
        flags.Add(newFlag);
    }

    public static void Add(this List<Command> subCommands)
    {
        Add(subCommands, "", "", new string[] { });
    }

    public static void Add(this List<Command> subCommands, string token, string description)
    {
        Add(subCommands, token, description, new string[] { });
    }

    public static void Add(this List<Command> subCommands, string token, string description,
        params string[] aliases)
    {
        var newCommand = new Command(token, description, aliases);
        subCommands.Add(newCommand);
    }
}
using System.Diagnostics;

namespace DewIt.Model.CommandLine;

/// <summary>
/// <i>Arguments</i>, or <i>args</i>, are positional parameters to a
/// command. <br/> For example, the file paths you provide to <c>cp</c> are
/// args. The order of args is often important: <c>cp foo bar</c>
/// means something different from <c>cp bar foo</c>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Argument
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }
    public Arity Arity { get; set; }

    public Argument() : this("", "", false, Arity.ONE)
    {
    }

    public Argument(string name, string description) : this(name, description, false, Arity.ONE)
    {
    }

    public Argument(string name, string description, bool isRequired) : this(name, description, isRequired, Arity.ONE)
    {
    }

    public Argument(string name, string description, bool isRequired, Arity arity)
    {
        Arity = arity;
        Description = description;
        IsRequired = isRequired;
        Name = name;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{Name} {IsRequiredDisplay}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string IsRequiredDisplay => IsRequired ? "(required)" : "";
}
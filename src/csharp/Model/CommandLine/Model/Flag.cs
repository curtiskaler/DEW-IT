using System.Diagnostics;

namespace DewIt.Model.CommandLine;

/// <summary>
/// <i>Flags</i> are named parameters, denoted with either
/// a hyphen and a single-letter name (<c>-r</c>) or a double hyphen
/// and a multiple-letter name (<c>--recursive</c>). They may or may
/// not also include a user-specified value (<c>--file foo.txt</c>,
/// or <c>--file=foo.txt</c>). The order of flags, generally speaking,
/// does not affect program semantics.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Flag : ArgumentHolder<Flag>
{
    public string Description { get; set; }
    public string LongName { get; set; }

    public List<string> ShortAliases { get; set; }

    public Flag(string longName, string description) : this(longName, description, new string[] { })
    {
    }

    public Flag(string longName, string description, params string[] aliases)
    {
        LongName = longName;
        ShortAliases = aliases.ToList();
        Description = description;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay =>
        $"{LongName} {ShortAliasesDisplay}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string ShortAliasesDisplay =>
        $"{(ShortAliases.Count != 0 ? "(" + string.Join(',', ShortAliases) + ")" : "")}";
}
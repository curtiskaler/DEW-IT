using System;
using B7.Types;

namespace B7.Lifecycle;

public class ExitCode : Enumeration<ExitCode>
{
    protected ExitCode(int ordinal, string name) : base(ordinal, name)
    {
    }

    public static readonly ExitCode SUCCESS = new ExitCode(0, nameof(SUCCESS));
}

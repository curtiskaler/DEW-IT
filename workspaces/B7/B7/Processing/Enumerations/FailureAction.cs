using B7.Types;

namespace B7.Processing;

public class FailureAction : Enumeration<FailureAction>
{

    protected FailureAction(int ordinal, string name) : base(ordinal, name)
    {
    }

    public static readonly FailureAction STOP = new(0, nameof(STOP));
    public static readonly FailureAction CONTINUE = new(1, nameof(CONTINUE));
}

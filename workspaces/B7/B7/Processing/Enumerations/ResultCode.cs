using B7.Types;

namespace B7.Processing;

public class ResultCode : Enumeration<FailureAction>
{

    protected ResultCode(int ordinal, string name) : base(ordinal, name)
    {
    }

    public static readonly ResultCode SUCCESS = new(0, nameof(SUCCESS));
    public static readonly ResultCode FAILURE = new(1, nameof(FAILURE));
    public static readonly ResultCode SKIP = new(2, nameof(SKIP));
}

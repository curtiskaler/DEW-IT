namespace B7.Processing;

public class Skipped : IResult
{
    public string Objective { get; }
    public ResultCode Code => ResultCode.SKIP;
    public string Reason { get; }

    public Skipped(string objective, string reason)
    {
        Objective = objective ?? throw new ArgumentNullException(nameof(objective));
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
    }

    public static implicit operator bool(Skipped result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        return true;
    }
}

#nullable enable

public class Skipped<TOut> : Skipped, IResult<TOut>
{
    public TOut? Output { get; }

    public Skipped(string objective, string reason) : base(objective, reason)
    {
        Output = default;
    }

    public static implicit operator bool(Skipped<TOut> result)
    {
        if (result == null) { throw new ArgumentNullException(nameof(result)); }
        return true;
    }
}

#nullable disable
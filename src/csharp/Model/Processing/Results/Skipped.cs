// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DewIt.Model.Processing.Results;

public class Skipped : IResult
{
    public string Objective { get; }
    public ResultCode Code => ResultCode.SKIP;
    public string Reason { get; }

    public Skipped(string objective, string reason)
    {
        Objective = objective;
        Reason = reason;
    }

    public static implicit operator bool(Skipped result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        return true;
    }
}

public class Skipped<TOut> : Skipped, IResult<TOut>
{
    public TOut? Output { get; }

    public Skipped(string objective, string reason) : base(objective, reason)
    {
        Output = default;
    }
}
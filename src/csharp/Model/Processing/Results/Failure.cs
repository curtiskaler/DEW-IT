// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DewIt.Model.Processing.Results;

public class Failure : IResult
{
    public ResultCode Code => ResultCode.FAILURE;
    public string Reason { get; }
    public List<Exception> Exceptions { get; }
    public FailureAction Severity { get; }


    public Failure(Exception ex) : this(null, ex.ToList(), FailureAction.STOP)
    {
    }

    public Failure(Exception ex, FailureAction severity) : this(null, ex.ToList(), severity)
    {
    }

    public Failure(List<Exception> exceptions, FailureAction severity) : this(null, exceptions, severity)
    {
    }

    public Failure(string reason, Exception ex) : this(reason, ex.ToList(), FailureAction.STOP)
    {
    }

    public Failure(string reason, List<Exception> exceptions) : this(reason, exceptions, FailureAction.STOP)
    {
    }

    public Failure(string reason, Exception ex, FailureAction severity) : this(reason, ex.ToList(), severity)
    {
    }

    public Failure(string? reason, List<Exception> exceptions, FailureAction severity)
    {
        Exceptions = exceptions;
        Reason = reason ?? "";
        Severity = severity;
    }

    public static implicit operator bool(Failure result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        return false;
    }
}

public class Failure<TOut> : Failure, IResult<TOut>
{
    public Failure(TOut output, Exception ex, FailureAction severity) : this(output, null, ex.ToList(), severity)
    {
    }

    public Failure(TOut output, List<Exception> exceptions, FailureAction severity) : this(output, null, exceptions,
        severity)
    {
    }

    public Failure(TOut output, string reason, Exception ex) : this(output, reason, ex.ToList())
    {
    }

    public Failure(TOut output, string reason, List<Exception> exceptions) : this(output, reason, exceptions,
        FailureAction.STOP)
    {
    }

    public Failure(TOut output, string reason, Exception ex, FailureAction severity) : this(output, reason, ex.ToList(),
        severity)
    {
    }

    public Failure(TOut output, string? reason, List<Exception> exceptions, FailureAction severity) : base(reason,
        exceptions,
        severity)
    {
        Output = output;
    }


    public TOut Output { get; }

    public static implicit operator bool(Failure<TOut> d) => d.Code != ResultCode.FAILURE;
}

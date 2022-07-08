// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DewIt.Model.Processing.Results;

public class Success : IResult
{
    public string Objective { get; }
    public ResultCode Code => ResultCode.SUCCESS;
    public string Message { get; }

    public Success(string objective) : this(objective, "")
    {
    }

    public Success(string objective, string message)
    {
        Objective = objective;
        Message = message;
    }

    public static implicit operator bool(Success result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        return true;
    }
}

public class Success<TOut> : Success, IResult<TOut>
{
    public TOut Output { get; }

    public Success(string objective, TOut output) : this(objective, output, "")
    {
    }

    public Success(string objective, TOut output, string message) : base(objective, message)
    {
        Output = output;
    }
}
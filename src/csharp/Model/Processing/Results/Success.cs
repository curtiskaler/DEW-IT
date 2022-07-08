// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DewIt.Model.Processing.Results;

public class Success : IResult
{
    public ResultCode Code => ResultCode.SUCCESS;
    public string Message { get; }

    public Success() : this("")
    {
    }

    public Success(string message)
    {
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

    public Success(TOut output) : this(output, "")
    {
    }

    public Success(TOut output, string message) : base(message)
    {
        Output = output;
    }
}
namespace B7.Processing;

public interface IResult
{
    string Objective { get; }
    ResultCode Code { get; }
}

public interface IResult<out TOut> : IResult
{
#nullable enable
    TOut? Output { get; }
#nullable disable
}
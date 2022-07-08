namespace DewIt.Model.Processing;

public interface IResult
{
    public string Objective { get; }
    public ResultCode Code { get; }
}

public interface IResult<out TOut> : IResult
{
    TOut? Output { get; }
}
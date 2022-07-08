namespace DewIt.Model.Processing;

public interface IResult
{
    public ResultCode Code { get; }
}

public interface IResult<out TOut> : IResult
{
    TOut Output { get; }
}
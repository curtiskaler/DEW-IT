namespace B7.Processing;

public interface IProcessResult
{
    ResultCode Code { get; }
    bool Failed { get; }
    bool Skipped { get; }
    bool Succeeded { get; }
    IStepAndResultCollection StepsAndResults { get; }
}

#nullable enable
public interface IProcessResult<out T> : IProcessResult
{
    T? Object { get; }
}
#nullable disable

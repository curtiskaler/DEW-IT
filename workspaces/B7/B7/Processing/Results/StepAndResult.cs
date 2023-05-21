namespace B7.Processing;

public class StepAndResult
{
    public IProcessStep Step { get; }
    public IResult Result { get; }

    public StepAndResult(IProcessStep step, IResult result)
    {
        Step = step ?? throw new ArgumentNullException(nameof(step));
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }

    public ResultCode Code => Result.Code;
    public bool Failed => Result.Code == ResultCode.FAILURE;
    public bool Skipped => Result.Code == ResultCode.SKIP;
    public bool Succeeded => Result.Code == ResultCode.SUCCESS;
}

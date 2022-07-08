namespace DewIt.Model.Processing.Results;

public class StepAndResult
{
    public StepAndResult(IProcessStep step, IResult result)
    {
        Step = step;
        Result = result;
    }

    public IProcessStep Step { get; }
    public IResult Result { get; }

    public ResultCode Code => Result.Code;
    public bool Failed => Result.Code == ResultCode.FAILURE;
    public bool Skipped => Result.Code == ResultCode.SKIP;
    public bool Succeeded => Result.Code == ResultCode.SUCCESS;
}
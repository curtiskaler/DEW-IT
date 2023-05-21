namespace B7.Processing.Results;

public class ProcessResult : IProcessResult
{
    public ProcessResult() : this(new List<StepAndResult>())
    {
    }

    public ProcessResult(IEnumerable<StepAndResult> stepsAndResults)
    {
        StepsAndResults = new StepAndResultCollection(stepsAndResults);
    }

    public ResultCode Code
    {
        get
        {
            if (Skipped) { return ResultCode.SKIP; }
            return Failed ? ResultCode.FAILURE : ResultCode.SUCCESS;
        }
    }

    public bool Failed => StepsAndResults.Any(it => it.Failed);

    public bool Skipped => StepsAndResults.All(it => it.Skipped);

    public bool Succeeded => StepsAndResults.All(it => it.Succeeded);

    public IStepAndResultCollection StepsAndResults { get; }
}

#nullable enable
public class ProcessResult<TOut> : ProcessResult, IProcessResult<TOut> where TOut : new()
{
    public TOut? Object { get; set; }
    public ProcessResult() : this(new TOut()) { }
    public ProcessResult(TOut? output) { Object = output; }
}
#nullable disable
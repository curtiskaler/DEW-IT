namespace B7.Processing;

public static class StepAndResultFactory
{
    public static StepAndResult Create(IProcessStep step, IResult result)
    {
        return new StepAndResult(step, result);
    }
}
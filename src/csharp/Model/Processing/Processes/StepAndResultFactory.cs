using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.Processes;

public static class StepAndResultFactory
{
    public static StepAndResult Create(IProcessStep step, IResult result)
    {
        return new StepAndResult(step, result);
    }
}
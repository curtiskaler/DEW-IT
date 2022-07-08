using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.Processes;

public class StepAndResultFactory: IStepAndResultFactory
{
    public StepAndResult Create(IProcessStep step, IResult result)
    {
        return new StepAndResult(step, result);
    }
}
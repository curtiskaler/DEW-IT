using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public interface IStepAndResultFactory
{
    StepAndResult Create(IProcessStep step, IResult result);
}
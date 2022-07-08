using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public interface IStepAndResultCollection : IList<StepAndResult>
{
    StepAndResult Get(IProcessStep step);
    StepAndResult Get(Guid stepUUID);
}
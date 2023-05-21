namespace B7.Processing;

public interface IStepAndResultCollection : IList<StepAndResult>
{
    StepAndResult Get(IProcessStep step);
    StepAndResult Get(Guid stepUUID);
}

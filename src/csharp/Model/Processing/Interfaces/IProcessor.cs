namespace DewIt.Model.Processing;

public interface IProcessor
{
    public IProcessResult RunSteps(IEnumerable<IProcessStep> steps);
}
namespace DewIt.Model.Processing;

public interface IProcessor: IProcessEvents
{
    public IProcessResult RunSteps(IEnumerable<IProcessStep> steps);
}

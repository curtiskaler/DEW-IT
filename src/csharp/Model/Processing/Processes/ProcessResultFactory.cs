namespace DewIt.Model.Processing.Processes;

public class ProcessResultFactory: IProcessResultFactory
{
    public ProcessResult Create()
    {
        return new ProcessResult();
    }
}
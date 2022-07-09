namespace DewIt.Model.Processing;

public interface IProcessValidator
{
    IResult ValidateStepsBeforeExecution(IEnumerable<IProcessStep> steps);
    IResult ValidateAndCleanupStep(int stepNumber, int count, IProcessStep step);
}

public interface IProcess 
{
    IProcessResult Execute();
}

namespace B7.Processing;

public interface IProcessValidator
{
    IResult ValidateStepsBeforeExecution(IEnumerable<IProcessStep> steps);
    IResult ValidateAndCleanupStep(int stepNumber, int count, IProcessStep step);
}

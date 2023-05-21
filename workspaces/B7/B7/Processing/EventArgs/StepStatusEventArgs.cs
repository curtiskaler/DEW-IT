namespace B7.Processing.EventArgs;

public class StepStatusEventArgs
{
    public IProcessStep Step { get; }
    public int StepNumber { get; }
    public int TotalSteps { get; }

    public StepStatusEventArgs(IProcessStep step, int stepNumber, int totalSteps)
    {
        Step = step ?? throw new ArgumentNullException(nameof(step));
        StepNumber = stepNumber;
        TotalSteps = totalSteps;
    }
}

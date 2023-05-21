namespace B7.Processing.EventArgs;

public class CompletedEventArgs : StepStatusEventArgs
{
    public Success Result { get; }

    public CompletedEventArgs(IProcessStep step, Success result, int stepNumber, int totalSteps) : base(step, stepNumber, totalSteps)
    {
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }
}

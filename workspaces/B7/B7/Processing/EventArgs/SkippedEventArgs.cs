namespace B7.Processing.EventArgs;

public class SkippedEventArgs : StepStatusEventArgs
{
    public string Reason { get; }
    public SkippedEventArgs(IProcessStep step, string reason, int stepNumber, int totalSteps) : base(step, stepNumber, totalSteps)
    {
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
    }
}

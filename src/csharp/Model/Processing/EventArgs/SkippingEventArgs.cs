// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DewIt.Model.Processing.EventArgs;

public class SkippingEventArgs : StepStatusEventArgs
{
    public string Reason { get; }

    public SkippingEventArgs(IProcessStep step, string reason, int stepNumber, int totalSteps) : base(step, stepNumber, totalSteps)
    {
        Reason = reason;
    }
}
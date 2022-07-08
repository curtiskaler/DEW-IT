// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.EventArgs;

public class StepCompletedEventArgs : StepStatusEventArgs
{
    public Success Result { get; }
    public StepCompletedEventArgs(IProcessStep step, Success result, int stepNumber, int totalSteps) : base(step, stepNumber, totalSteps)
    {
        Result = result;
    }
}
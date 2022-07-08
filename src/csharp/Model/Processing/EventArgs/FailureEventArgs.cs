using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.EventArgs;

public class FailureEventArgs : StepStatusEventArgs
{
    public List<Exception> Exceptions => Failure.Exceptions;
    public Failure Failure { get; }
    public string Reason => Failure.Reason;
    public FailureAction Severity => Failure.Severity;


    public FailureEventArgs(IProcessStep step, Failure failure, int stepNumber, int totalSteps)
        : base(step, stepNumber, totalSteps)
    {
        Failure = failure;
    }
}
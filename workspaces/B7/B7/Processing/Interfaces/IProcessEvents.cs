using B7.Processing.EventArgs;

namespace B7.Processing;

#nullable enable

public interface IProcessEvents
{
    event EventHandler? ProcessStarting;
    event EventHandler? ProcessComplete;

    event EventHandler<StepStatusEventArgs>? StepStarting;
    event EventHandler<CompletedEventArgs>? StepComplete;
    event EventHandler<FailureEventArgs>? StepFailed;
    event EventHandler<SkippedEventArgs>? StepSkipped;

    event EventHandler<StepStatusEventArgs>? StepExecutionStarting;
    event EventHandler<CompletedEventArgs>? StepExecutionComplete;
    event EventHandler<FailureEventArgs>? StepExecutionFailed;
    event EventHandler<SkippedEventArgs>? StepExecutionSkipped;

    event EventHandler<StepStatusEventArgs>? StepValidationStarting;
    event EventHandler<CompletedEventArgs>? StepValidationComplete;
    event EventHandler<FailureEventArgs>? StepValidationFailed;
    event EventHandler<SkippedEventArgs>? StepValidationSkipped;
}
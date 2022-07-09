using DewIt.Model.Processing.EventArgs;

namespace DewIt.Model.Processing;

public interface IProcessEvents
{
    event EventHandler? ProcessStarting;
    event EventHandler? ProcessComplete;
    event EventHandler<StepCompletedEventArgs>? StepComplete;
    event EventHandler<FailureEventArgs>? StepFailed;
    event EventHandler<SkippingEventArgs>? StepSkipped;
    event EventHandler<StepStatusEventArgs>? StepStarting;
    event EventHandler<StepCompletedEventArgs>? StepExecutionComplete;
    event EventHandler<FailureEventArgs>? StepExecutionFailed;
    event EventHandler<SkippingEventArgs>? StepExecutionSkipped;
    event EventHandler<StepStatusEventArgs>? StepExecutionStarting;
    event EventHandler<StepCompletedEventArgs>? StepValidationComplete;
    event EventHandler<FailureEventArgs>? StepValidationFailed;
    event EventHandler<SkippingEventArgs>? StepValidationSkipped;
    event EventHandler<StepStatusEventArgs>? StepValidationStarting;
}
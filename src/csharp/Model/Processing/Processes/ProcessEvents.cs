// ReSharper disable UnusedType.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.InteropServices;
using DewIt.Model.Processing.EventArgs;
using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.Processes;

// TODO: disposable?

public abstract class ProcessEvents
{
    public event EventHandler? ProcessStarting;
    public event EventHandler? ProcessComplete;

    public event EventHandler<StepCompletedEventArgs>? StepComplete;
    public event EventHandler<FailureEventArgs>? StepFailed;
    public event EventHandler<SkippingEventArgs>? StepSkipped;
    public event EventHandler<StepStatusEventArgs>? StepStarting;

    public event EventHandler<StepCompletedEventArgs>? StepExecutionComplete;
    public event EventHandler<FailureEventArgs>? StepExecutionFailed;
    public event EventHandler<SkippingEventArgs>? StepExecutionSkipped;
    public event EventHandler<StepStatusEventArgs>? StepExecutionStarting;

    public event EventHandler<StepCompletedEventArgs>? StepValidationComplete;
    public event EventHandler<FailureEventArgs>? StepValidationFailed;
    public event EventHandler<SkippingEventArgs>? StepValidationSkipped;
    public event EventHandler<StepStatusEventArgs>? StepValidationStarting;

    // Process Event Handlers
    protected void OnProcessStarting([Optional] object? sender, [Optional] System.EventArgs? e)
    {
        sender ??= this;
        e ??= System.EventArgs.Empty;
        ProcessStarting?.Invoke(sender, e);
    }

    protected void OnProcessComplete([Optional] object? sender, [Optional] System.EventArgs? e)
    {
        sender ??= this;
        e ??= System.EventArgs.Empty;
        ProcessComplete?.Invoke(sender, e);
    }

    // Step Overview Event Handlers
    protected void OnStepComplete(object? sender, StepCompletedEventArgs e)
    {
        sender ??= this;
        StepComplete?.Invoke(sender, e);
    }

    protected void OnStepComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepComplete(this, new StepCompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepFailed(IProcessStep step, Failure result, int stepNumber, int totalSteps)
    {
        OnStepFailed(this,
            new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepFailed(object? sender, FailureEventArgs e)
    {
        sender ??= this;
        StepFailed?.Invoke(sender, e);
    }

    protected void OnStepSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepSkipped(this, new SkippingEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepSkipped(object? sender, SkippingEventArgs e)
    {
        StepSkipped?.Invoke(sender, e);
    }

    protected void OnStepStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepStarting(object? sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepStarting?.Invoke(sender, e);
    }

    // Execution Event Handlers
    protected void OnStepExecutionComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepExecutionComplete(this, new StepCompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepExecutionComplete(object? sender, StepCompletedEventArgs e)
    {
        sender ??= this;
        StepExecutionComplete?.Invoke(sender, e);
    }

    protected void OnStepExecutionFailed(IProcessStep step, Failure result, int stepNumber, int totalSteps)
    {
        OnStepExecutionFailed(this,
            new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepExecutionFailed(object? sender, FailureEventArgs e)
    {
        sender ??= this;
        StepExecutionFailed?.Invoke(sender, e);
    }

    protected void OnStepExecutionSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepExecutionSkipped(this,
            new SkippingEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepExecutionSkipped(object? sender, SkippingEventArgs e)
    {
        sender ??= this;
        StepExecutionSkipped?.Invoke(sender, e);
    }

    protected void OnStepExecutionStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepExecutionStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepExecutionStarting(object? sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepExecutionStarting?.Invoke(sender, e);
    }

    // Validation Event Handlers
    protected void OnStepValidationComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepValidationComplete(this, new StepCompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepValidationComplete(object? sender, StepCompletedEventArgs e)
    {
        sender ??= this;
        StepValidationComplete?.Invoke(sender, e);
    }

    protected void OnStepValidationFailed(IProcessStep step, Failure result, int stepNumber,
        int totalSteps)
    {
        OnStepValidationFailed(this,
            new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepValidationFailed(object? sender, FailureEventArgs e)
    {
        sender ??= this;
        StepValidationFailed?.Invoke(sender, e);
    }

    protected void OnStepValidationSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepValidationSkipped(this,
            new SkippingEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepValidationSkipped(object? sender, SkippingEventArgs e)
    {
        sender ??= this;
        StepValidationSkipped?.Invoke(sender, e);
    }

    protected void OnStepValidationStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepValidationStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepValidationStarting(object? sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepValidationStarting?.Invoke(sender, e);
    }
}
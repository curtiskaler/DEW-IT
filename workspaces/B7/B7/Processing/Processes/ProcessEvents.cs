using System.Runtime.InteropServices;
using B7.Processing.EventArgs;
using Microsoft.Extensions.Logging;

namespace B7.Processing.Processes;

#nullable enable

public class ProcessEvents : IProcessEvents
{
    protected ILogger _logger;

    protected ProcessEvents(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public event EventHandler? ProcessStarting;
    public event EventHandler? ProcessComplete;

    public event EventHandler<StepStatusEventArgs>? StepStarting;
    public event EventHandler<CompletedEventArgs>? StepComplete;
    public event EventHandler<FailureEventArgs>? StepFailed;
    public event EventHandler<SkippedEventArgs>? StepSkipped;

    public event EventHandler<StepStatusEventArgs>? StepExecutionStarting;
    public event EventHandler<CompletedEventArgs>? StepExecutionComplete;
    public event EventHandler<FailureEventArgs>? StepExecutionFailed;
    public event EventHandler<SkippedEventArgs>? StepExecutionSkipped;

    public event EventHandler<StepStatusEventArgs>? StepValidationStarting;
    public event EventHandler<CompletedEventArgs>? StepValidationComplete;
    public event EventHandler<FailureEventArgs>? StepValidationFailed;
    public event EventHandler<SkippedEventArgs>? StepValidationSkipped;

    #region Process Event Handlers

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

    #endregion

    #region Step Overview Event Handlers

    protected void OnStepStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepStarting(object sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepStarting?.Invoke(sender, e);
    }

    protected void OnStepComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepComplete(this, new CompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepComplete(object sender, CompletedEventArgs e)
    {
        sender ??= this;
        StepComplete?.Invoke(sender, e);
    }

    protected void OnStepFailed(IProcessStep step, Failure result, int stepNumber, int totalSteps)
    {
        OnStepFailed(this, new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepFailed(object sender, FailureEventArgs e)
    {
        sender ??= this;
        StepFailed?.Invoke(sender, e);
    }

    protected void OnStepSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepSkipped(this, new SkippedEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepSkipped(object sender, SkippedEventArgs e)
    {
        sender ??= this;
        StepSkipped?.Invoke(sender, e);
    }

    #endregion

    #region Execution Event Handlers

    protected void OnStepExecutionStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepExecutionStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepExecutionStarting(object sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepExecutionStarting?.Invoke(sender, e);
    }

    protected void OnStepExecutionComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepExecutionComplete(this, new CompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepExecutionComplete(object sender, CompletedEventArgs e)
    {
        sender ??= this;
        StepExecutionComplete?.Invoke(sender, e);
    }

    protected void OnStepExecutionFailed(IProcessStep step, Failure result, int stepNumber, int totalSteps)
    {
        OnStepExecutionFailed(this, new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepExecutionFailed(object sender, FailureEventArgs e)
    {
        sender ??= this;
        StepExecutionFailed?.Invoke(sender, e);
    }

    protected void OnStepExecutionSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepExecutionSkipped(this, new SkippedEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepExecutionSkipped(object sender, SkippedEventArgs e)
    {
        sender ??= this;
        StepExecutionSkipped?.Invoke(sender, e);
    }

    #endregion

    #region Validation Event Handlers

    protected void OnStepValidationStarting(IProcessStep step, int stepNumber, int totalSteps)
    {
        OnStepValidationStarting(this, new StepStatusEventArgs(step, stepNumber, totalSteps));
    }

    protected void OnStepValidationStarting(object sender, StepStatusEventArgs e)
    {
        sender ??= this;
        StepValidationStarting?.Invoke(sender, e);
    }

    protected void OnStepValidationComplete(IProcessStep step, Success result, int stepNumber, int totalSteps)
    {
        OnStepValidationComplete(this, new CompletedEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepValidationComplete(object sender, CompletedEventArgs e)
    {
        sender ??= this;
        StepValidationComplete?.Invoke(sender, e);
    }

    protected void OnStepValidationFailed(IProcessStep step, Failure result, int stepNumber, int totalSteps)
    {
        OnStepValidationFailed(this, new FailureEventArgs(step, result, stepNumber, totalSteps));
    }

    protected void OnStepValidationFailed(object sender, FailureEventArgs e)
    {
        sender ??= this;
        StepValidationFailed?.Invoke(sender, e);
    }

    protected void OnStepValidationSkipped(IProcessStep step, Skipped result, int stepNumber, int totalSteps)
    {
        OnStepValidationSkipped(this, new SkippedEventArgs(step, result.Reason, stepNumber, totalSteps));
    }

    protected void OnStepValidationSkipped(object sender, SkippedEventArgs e)
    {
        sender ??= this;
        StepValidationSkipped?.Invoke(sender, e);
    }

    #endregion
}


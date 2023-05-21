
using System;
using B7.Processing.EventArgs;
using B7.Processing.Processes;
using B7.Types;
using Microsoft.Extensions.Logging;

namespace B7.Processing;


public interface IProcessor
{
    IProcessResult RunSteps(IEnumerable<IProcessStep> steps);
}

public class Processor : ProcessEvents, IProcessor
{
    public Processor(ILogger logger) : base(logger)
    {
    }

    public IProcessResult RunSteps(IEnumerable<IProcessStep> steps)
    {
        var process = new Process(steps);
        ForwardEvents(process);
        return process.Execute();
    }

    protected void ForwardEvents(IProcessEvents process)
    {
        process.ProcessComplete += LogInvocation(_logger, OnProcessComplete);
        process.ProcessStarting += LogInvocation(_logger, OnProcessStarting);

        process.StepComplete += LogInvocation<CompletedEventArgs>(_logger, OnStepComplete);
        process.StepFailed += LogInvocation<FailureEventArgs>(_logger, OnStepFailed);
        process.StepSkipped += LogInvocation<SkippedEventArgs>(_logger, OnStepSkipped);
        process.StepStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepStarting);

        process.StepExecutionComplete += LogInvocation<CompletedEventArgs>(_logger, OnStepExecutionComplete);
        process.StepExecutionFailed += LogInvocation<FailureEventArgs>(_logger, OnStepExecutionFailed);
        process.StepExecutionSkipped += LogInvocation<SkippedEventArgs>(_logger, OnStepExecutionSkipped);
        process.StepExecutionStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepExecutionStarting);

        process.StepValidationComplete += LogInvocation<CompletedEventArgs>(_logger, OnStepValidationComplete);
        process.StepValidationFailed += LogInvocation<FailureEventArgs>(_logger, OnStepValidationFailed);
        process.StepValidationSkipped += LogInvocation<SkippedEventArgs>(_logger, OnStepValidationSkipped);
        process.StepValidationStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepValidationStarting);
    }


#nullable enable
    private static EventHandler LogInvocation(ILogger logger, EventHandler? handler)
    {
        void NewEventHandler(object? sender, System.EventArgs args)
        {
            if (handler == null) return;
            logger.Log(LogLevel.Trace, handler.Method.Name, Array.Empty<object>());
            handler.Invoke(sender, args);
        }
        return NewEventHandler;
    }

    private static EventHandler<TEventArgs> LogInvocation<TEventArgs>(ILogger logger, EventHandler<TEventArgs>? handler)
    {
        void NewEventHandler(object? sender, TEventArgs args)
        {
            if (handler == null) return;
            logger.Log(LogLevel.Trace, handler.Method.Name, Array.Empty<object>());
            handler.Invoke(sender, args);
        }
        return NewEventHandler;
    }

#nullable disable
}


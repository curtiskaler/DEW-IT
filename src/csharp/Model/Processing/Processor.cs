using DewIt.Model.Processing.EventArgs;
using DewIt.Model.Processing.Processes;
using Microsoft.Extensions.Logging;

namespace DewIt.Model.Processing;

// TODO: Do we need something to be disposable here?

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

        process.StepComplete += LogInvocation<StepCompletedEventArgs>(_logger, OnStepComplete);
        process.StepFailed += LogInvocation<FailureEventArgs>(_logger, OnStepFailed);
        process.StepSkipped += LogInvocation<SkippingEventArgs>(_logger, OnStepSkipped);
        process.StepStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepStarting);

        process.StepExecutionComplete += LogInvocation<StepCompletedEventArgs>(_logger, OnStepExecutionComplete);
        process.StepExecutionFailed += LogInvocation<FailureEventArgs>(_logger, OnStepExecutionFailed);
        process.StepExecutionSkipped += LogInvocation<SkippingEventArgs>(_logger, OnStepExecutionSkipped);
        process.StepExecutionStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepExecutionStarting);

        process.StepValidationComplete += LogInvocation<StepCompletedEventArgs>(_logger, OnStepValidationComplete);
        process.StepValidationFailed += LogInvocation<FailureEventArgs>(_logger, OnStepValidationFailed);
        process.StepValidationSkipped += LogInvocation<SkippingEventArgs>(_logger, OnStepValidationSkipped);
        process.StepValidationStarting += LogInvocation<StepStatusEventArgs>(_logger, OnStepValidationStarting);
    }

    private static EventHandler LogInvocation(ILogger logger, EventHandler? handler)
    {
        void NewEventHandler(object? sender, System.EventArgs args)
        {
            if (handler == null) return;
            logger.Log(LogLevel.Information, handler.Method.Name, Array.Empty<object>());
            handler.Invoke(sender, args);
        }

        return NewEventHandler;
    }

    private static EventHandler<TEventArgs> LogInvocation<TEventArgs>(ILogger logger, EventHandler<TEventArgs>? handler)
    {
        void NewEventHandler(object? sender, TEventArgs args)
        {
            if (handler == null) return;
            logger.Log(LogLevel.Information, handler.Method.Name, Array.Empty<object>());
            handler.Invoke(sender, args);
        }

        return NewEventHandler;
    }
}

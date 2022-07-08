using DewIt.Model.Processing.Processes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DewIt.Model.Processing;

// TODO: Do we need something to be disposable here?

public sealed class Processor : ProcessEvents, IProcessor
{
    private bool _silent;

    public Processor() : this(NullLogger.Instance, true)
    {
    }

    public Processor(ILogger logger, bool silent = false) : base(logger)
    {
        _silent = silent;
    }


    public IProcessResult RunSteps(IEnumerable<IProcessStep> steps)
    {
        var process = new Process(steps);
        ForwardEvents(process);
        return process.Execute();
    }

    private void ForwardEvents(ProcessEvents process)
    {
        process.ProcessComplete += OnProcessComplete;
        process.ProcessStarting += OnProcessStarting;

        process.StepComplete += OnStepComplete;
        process.StepFailed += OnStepFailed;
        process.StepSkipped += OnStepSkipped;
        process.StepStarting += OnStepStarting;

        process.StepExecutionComplete += OnStepExecutionComplete;
        process.StepExecutionFailed += OnStepExecutionFailed;
        process.StepExecutionSkipped += OnStepExecutionSkipped;
        process.StepExecutionStarting += OnStepExecutionStarting;

        process.StepValidationComplete += OnStepValidationComplete;
        process.StepValidationFailed += OnStepValidationFailed;
        process.StepValidationSkipped += OnStepValidationSkipped;
        process.StepValidationStarting += OnStepValidationStarting;
    }
}
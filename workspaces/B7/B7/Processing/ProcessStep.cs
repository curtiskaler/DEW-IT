using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace B7.Processing;

public abstract class ProcessStep : IProcessStep
{
    protected ILogger _logger { get; init; }

    /// <summary> A unique identifier. </summary>
    public Guid UUID { get; init; }

    /// <summary> A short text description of what this step does. </summary>
    public abstract string Title { get; }

    /// <summary>
    /// Determine whether this step is necessary at all.
    /// For example, if the action is not idempotent, then check if it's ever been run before.
    /// </summary>
    public virtual bool IsNecessary() => true;

    /// <summary> The expected action when the execution of this step fails. </summary>
    public virtual FailureAction ExecutionFailureAction => FailureAction.STOP;

    /// <summary> The expected action when the post-execution validation and cleanup for this step fails. </summary>
    public virtual FailureAction PostExecutionValidationFailureAction => FailureAction.STOP;

    /// <summary> Execute the step. </summary>
    public abstract IResult Execute(IProcessResult resultsOfPreviousSteps);

    /// <summary> Verify that everything got done correctly, and do any cleanup. </summary>
    public IResult ValidateAndCleanup() => new Success(string.Format(ProcessingStrings.fmt_ValidateAndCleanup, Title));

    protected ProcessStep() : this(NullLogger.Instance) { }
    protected ProcessStep(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}


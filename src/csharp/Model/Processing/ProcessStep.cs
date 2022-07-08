using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DewIt.Model.Processing;

public abstract class ProcessStep : IProcessStep
{
    protected ILogger Logger { get; }

    protected static IResultFactory ResultFactory { get; set; } = new ResultFactory();

    /// <summary> Execute the step. </summary>
    public abstract IResult Execute(IStepAndResultCollection previousSteps);

    public Guid UUID { get; init; }

    /// <summary> A short text description of what this step does. </summary>
    public abstract string Title { get; }

    /// <summary> Determine whether the step is necessary at all.
    /// For example, if the action is publish the system namespace, then just check if it's ever been run before.
    /// </summary>
    public virtual bool IsNecessary() => true;

    /// <summary> The expected action when this step fails. </summary>
    public virtual FailureAction ExecutionFailureAction => FailureAction.STOP;

    /// <summary> The expected action when the post-execution validation and cleanup for this step fails. </summary>
    public virtual FailureAction PostExecutionValidationFailureAction => FailureAction.STOP;

    /// <summary> Verify that everything got done correctly, and do any cleanup. </summary>
    public virtual IResult ValidateAndCleanup() => new Success(string.Format(ProcessingStrings.fmt_ValidateAndCleanup, Title));
    
    protected ProcessStep() : this(NullLogger.Instance)
    {
    }

    protected ProcessStep(ILogger logger)
    {
        Logger = logger;
    }
}

public abstract class ProcessStep<TOut> : ProcessStep
{
    /// <summary> Execute the step, and include the output with the result. </summary>
    public abstract override IResult<TOut> Execute(IStepAndResultCollection previousSteps);

    protected ProcessStep() : base(NullLogger.Instance)
    {
    }
    protected ProcessStep(ILogger logger) : base(logger)
    {
    }
}
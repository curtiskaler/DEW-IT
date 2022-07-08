using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public abstract class ProcessStep : IProcessStep
{
    /// <summary> Execute the step. </summary>
    public abstract IResult Execute();

    /// <summary> A short text description of what this step does. </summary>
    public abstract string Title { get; }

    /// <summary> Determine whether the step is necessary at all.
    /// For example, if the action is publish the system namespace, then just check if it's ever been run before.
    /// </summary>
    public virtual bool IsNecessary() => true;

    /// <summary> The expected action when this step fails. </summary>
    public abstract FailureAction ExecutionFailureAction { get; }

    /// <summary> The expected action when the post-execution validation and cleanup for this step fails. </summary>
    public abstract FailureAction PostExecutionValidationFailureAction { get; }

    /// <summary> Verify that everything got done correctly, and do any cleanup. </summary>
    public virtual IResult ValidateAndCleanup() => new Success();
}

public abstract class ProcessStep<TOut> : ProcessStep
{
    /// <summary> Execute the step, and include the output with the result. </summary>
    public abstract override IResult<TOut> Execute();
}
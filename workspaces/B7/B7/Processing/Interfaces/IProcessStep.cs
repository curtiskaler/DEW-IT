namespace B7.Processing;

public interface IProcessStep
{
    /// <summary>
    /// A unique string identifier for use in identifying this step.
    /// </summary>
    Guid UUID { get; }

    /// <summary>
    /// A short text description of what this step does.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Determine whether this step is necessary at all.
    /// </summary>
    /// <returns></returns>
    bool IsNecessary();

    /// <summary>
    /// The expected action when this step fails.
    /// </summary>
    FailureAction ExecutionFailureAction { get; }

    /// <summary>
    /// The expected action when the post-execution validation and cleanup fails.
    /// </summary>
    FailureAction PostExecutionValidationFailureAction { get; }

    /// <summary>
    /// Verify that everything got done correctly, and do any cleanup.
    /// </summary>
    /// <returns></returns>
    IResult ValidateAndCleanup();

    /// <summary> Execute the step. </summary>
    /// <param name="resultsOfPreviousSteps"></param>
    /// <returns></returns>
    IResult Execute(IProcessResult resultsOfPreviousSteps);
}

namespace DewIt.Model.Processing;

public interface IProcessStep 
{
    /// <summary> A short text description of what this step does. </summary>
    string Title { get; }

    /// <summary> Determine whether the step is necessary at all. </summary>
    bool IsNecessary();

    /// <summary> The expected action when this step fails. </summary>
    FailureAction ExecutionFailureAction { get; }

    /// <summary> The expected action when the post-execution validation and cleanup for this step fails. </summary>
    FailureAction PostExecutionValidationFailureAction { get; }

    /// <summary> Verify that everything got done correctly, and do any cleanup. </summary>
    IResult ValidateAndCleanup();

    /// <summary> Execute the step. </summary>
    IResult Execute();
}

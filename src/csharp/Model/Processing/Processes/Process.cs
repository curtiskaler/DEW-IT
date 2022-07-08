using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing.Processes;

public sealed class Process: ProcessEvents
{
    private readonly List<IProcessStep> _steps;
    private readonly IProcessResultFactory _processResultFactory;
    private readonly IStepAndResultFactory _stepAndResultFactory;
    private readonly IResultFactory _resultFactory;

    public Process(IEnumerable<IProcessStep> steps) : this(steps, new ProcessResultFactory(),
        new StepAndResultFactory(), new ResultFactory())
    {
    }

    public Process(IEnumerable<IProcessStep> steps,
        IProcessResultFactory processResultFactory,
        IStepAndResultFactory stepAndResultFactory,
        IResultFactory resultFactory)
    {
        _steps = steps.ToList();
        _stepAndResultFactory = stepAndResultFactory;
        _processResultFactory = processResultFactory;
        _resultFactory = resultFactory;
    }


    public IResult ValidateSteps()
    {
        // do some validation around 
        return this._resultFactory.SUCCESS();
    }

    public ProcessResult Execute()
    {
        var total = _steps.Count;

        OnProcessStarting();

        var processResult = _processResultFactory.Create();

        for (var index = 0; index < total; index++)
        {
            var stepNumber = index + 1;
            var step = _steps[index];

            var necessary = step.IsNecessary();
            if (!necessary)
            {
                var skipped = _resultFactory.SKIP(ProcessingStrings.REASON_Step_not_Necessary);
                OnStepSkipped(step, skipped, stepNumber, total);
                continue;
            }

            OnStepStarting(step, stepNumber, total);

            var execution = ExecuteStep(stepNumber, total, step);
            if (execution.IsFailure())
            {
                var stepAndResult = _stepAndResultFactory.Create(step, execution);
                processResult.StepsAndResults.Add(stepAndResult);
                return processResult;
            }

            var validation = ValidateAndCleanupStep(stepNumber, total, step);
            if (validation.IsFailure())
            {
                var result = (validation as Failure)!;
                var stepAndResult = _stepAndResultFactory.Create(step, result);
                processResult.StepsAndResults.Add(stepAndResult);
                return processResult;
            }

            OnStepComplete(step, (execution as Success)!, stepNumber, total);

            var executionStepAndResult = _stepAndResultFactory.Create(step, execution);
            processResult.StepsAndResults.Add(executionStepAndResult);
        }

        OnProcessComplete();
        return processResult;
    }

    protected IResult ValidateAndCleanupStep(int stepNumber, int count, IProcessStep step)
    {
        OnStepValidationStarting(step, stepNumber, count);
        try
        {
            var validation = step.ValidateAndCleanup();
            if (validation.IsSkipped())
            {
                OnStepValidationSkipped(step, (validation as Skipped)!, stepNumber, count);
            }
            else if (validation.IsSuccess())
            {
                OnStepValidationComplete(step, (validation as Success)!, stepNumber, count);
            }
            else
            {
                OnStepValidationFailed(step, (validation as Failure)!, stepNumber, count);
            }

            return validation;
        }
        catch (Exception ex)
        {
            var result =_resultFactory.FAILURE(ex.Message, ex, step.PostExecutionValidationFailureAction);
            OnStepValidationFailed(step, result, stepNumber, count);
            return result;
        }
    }

    protected IResult ExecuteStep(int stepNumber, int count, IProcessStep step)
    {
        OnStepExecutionStarting(step, stepNumber, count);
        try
        {
            var execution = step.Execute();
            if (execution.IsSkipped())
            {
                OnStepExecutionSkipped(step, (execution as Skipped)!, stepNumber, count);
            }
            else if (execution.IsSuccess())
            {
                OnStepExecutionComplete(step, (execution as Success)!, stepNumber, count);
            }
            else
            {
                OnStepExecutionFailed(step, (execution as Failure)!, stepNumber, count);
            }

            return execution;
        }
        catch (Exception ex)
        {
            var result =_resultFactory.FAILURE(ex.Message, ex, step.ExecutionFailureAction);
            OnStepFailed(step, result, stepNumber, count);
            return result;
        }
    }
}

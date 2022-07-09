using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DewIt.Model.Processing.Processes;

public sealed class Process : ProcessEvents, IProcess
{
    private readonly IProcessValidator _validator;
    private readonly List<IProcessStep> _steps;

    public Process(IEnumerable<IProcessStep> steps) : this(NullLogger.Instance, steps)
    {
    }

    public Process(ILogger logger, IEnumerable<IProcessStep> steps)
        : this(logger, new ProcessValidator(logger), steps)
    {
    }

    public Process(ILogger logger, IProcessValidator validator, IEnumerable<IProcessStep> steps)
        : base(logger)
    {
        _validator = validator;
        _steps = steps.ToList();
    }

    public IProcessResult Execute()
    {
        var processResult = ProcessResultFactory.Create();
        OnProcessStarting();

        var total = _steps.Count;
        for (var index = 0; index < total; index++)
        {
            var stepNumber = index + 1;
            var step = _steps[index];

            var necessary = step.IsNecessary();
            if (!necessary)
            {
                var skipped = ResultFactory.SKIP(step.Title, ProcessingStrings.REASON_Step_not_Necessary);
                OnStepSkipped(step, skipped, stepNumber, total);
                continue;
            }

            OnStepStarting(step, stepNumber, total);

            var execution = ExecuteStep(stepNumber, total, step, processResult);
            if (execution.IsFailure())
            {
                var stepAndResult = StepAndResultFactory.Create(step, execution);
                processResult.StepsAndResults.Add(stepAndResult);
                return processResult;
            }

            var validation = _validator.ValidateAndCleanupStep(stepNumber, total, step);
            if (validation.IsFailure())
            {
                var failure = (validation as Failure)!;
                var stepAndResult = StepAndResultFactory.Create(step, failure);
                processResult.StepsAndResults.Add(stepAndResult);
                return processResult;
            }

            OnStepComplete(step, (execution as Success)!, stepNumber, total);

            var executionStepAndResult = StepAndResultFactory.Create(step, execution);
            processResult.StepsAndResults.Add(executionStepAndResult);
        }

        OnProcessComplete();
        return processResult;
    }

    private IResult ExecuteStep(int stepNumber, int count, IProcessStep step, IProcessResult result)
    {
        OnStepExecutionStarting(step, stepNumber, count);

        try
        {
            var execution = step.Execute(result);

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
            var failure = ResultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepFailed(step, failure, stepNumber, count);
            return failure;
        }
    }
}
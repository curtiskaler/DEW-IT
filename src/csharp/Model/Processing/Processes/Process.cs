using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DewIt.Model.Processing.Processes;

// TODO: make previous step results available to next step, to prevent forcing rework

public sealed class Process : ProcessEvents
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
        : this(steps, processResultFactory, stepAndResultFactory, resultFactory, NullLogger.Instance)
    {
    }

    public Process(IEnumerable<IProcessStep> steps,
        IProcessResultFactory processResultFactory,
        IStepAndResultFactory stepAndResultFactory,
        IResultFactory resultFactory,
        ILogger logger
    ) : base(logger)
    {
        _steps = steps.ToList();
        _stepAndResultFactory = stepAndResultFactory;
        _processResultFactory = processResultFactory;
        _resultFactory = resultFactory;
    }
    
    public IResult ValidateSteps()
    {
        var objective = ProcessingStrings.msg_Validating_Steps;

        // ensure all step UUIDs are unique
        var duplicates = _steps.GroupBy(it => it.UUID)
            .Where(g => g.Count() > 1)
            .Select(it => it.Key.ToString())
            .ToList();


        if (duplicates.Any())
        {
            var dupString = string.Join(", ", duplicates);
            var reason = string.Format(ProcessingStrings.ERROR_Steps_not_unique, dupString);
            var ex = new InvalidOperationException(reason);
            return _resultFactory.FAILURE(objective, reason, ex);
        }

        return _resultFactory.SUCCESS(objective);
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
                var skipped = _resultFactory.SKIP(step.Title, ProcessingStrings.REASON_Step_not_Necessary);
                OnStepSkipped(step, skipped, stepNumber, total);
                continue;
            }

            OnStepStarting(step, stepNumber, total);

            var execution = ExecuteStep(stepNumber, total, step, processResult.StepsAndResults);
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

    private IResult ValidateAndCleanupStep(int stepNumber, int count, IProcessStep step)
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
            var result = _resultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepValidationFailed(step, result, stepNumber, count);
            return result;
        }
    }

    private IResult ExecuteStep(int stepNumber, int count, IProcessStep step, IStepAndResultCollection previousSteps)
    {
        OnStepExecutionStarting(step, stepNumber, count);
        try
        {
            var execution = step.Execute(previousSteps);
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
            var result = _resultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepFailed(step, result, stepNumber, count);
            return result;
        }
    }
}
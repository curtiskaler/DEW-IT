using System.Globalization;
using B7.Processing.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace B7.Processing.Processes;

public class Process : ProcessEvents, IProcess
{
    private ProcessValidator processValidator;
    private List<IProcessStep> steps;

    public Process(IEnumerable<IProcessStep> steps) : this(NullLogger.Instance, steps)
    {
    }

    public Process(ILogger logger, IEnumerable<IProcessStep> steps) : this(logger, new ProcessValidator(logger), steps)
    {
    }

    public Process(ILogger logger, ProcessValidator processValidator, IEnumerable<IProcessStep> steps) : base(logger)
    {
        this.processValidator = processValidator;
        this.steps = steps.ToList();
    }

    public IProcessResult Execute()
    {
        IProcessResult processResult = ProcessResultFactory.Create();
        OnProcessStarting();

        int total = steps.Count;
        for (int index = 0; index < total; index++)
        {
            int stepNumber = index + 1;
            IProcessStep step = steps[index];

            bool necessary = step.IsNecessary();
            if (!necessary)
            {
                Skipped result = ResultFactory.SKIP(step.Title, String.Format(ProcessingStrings.REASON_StepNotNecessary, CultureInfo.CurrentCulture));
                OnStepSkipped(step, result, stepNumber, total);
                continue;
            }

            OnStepStarting(step, stepNumber, total);

            IResult execution = ExecuteStep(stepNumber, total, step, processResult);
            if (execution.IsFailure())
            {
                var stepAndResult = StepAndResultFactory.Create(step, execution);
                processResult.StepsAndResults.Add(stepAndResult);
                return processResult;
            }

            var validation = processValidator.ValidateAndCleanupStep(stepNumber, total, step);
            if (validation.IsFailure())
            {
                var stepAndResult = StepAndResultFactory.Create(step, validation);
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

    private IResult ExecuteStep(int stepNumber, int totalSteps, IProcessStep step, IProcessResult processResult)
    {
        OnStepExecutionStarting(step, stepNumber, totalSteps);

        try
        {
            IResult execution = step.Execute(processResult);
            if (execution.IsSkipped())
            {
                OnStepExecutionSkipped(step, (execution as Skipped)!, stepNumber, totalSteps);
            }
            else if (execution.IsSuccess())
            {
                OnStepExecutionComplete(step, (execution as Success)!, stepNumber, totalSteps);
            }
            else
            {
                OnStepExecutionFailed(step, (execution as Failure)!, stepNumber, totalSteps);
            }
            return execution;
        }
        catch (Exception ex)
        {
            Failure failure = ResultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepFailed(step, failure, stepNumber, totalSteps);
            return failure;
        }
    }
}

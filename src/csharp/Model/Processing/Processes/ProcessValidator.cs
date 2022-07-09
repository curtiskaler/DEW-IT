using DewIt.Model.Processing.Results;
using Microsoft.Extensions.Logging;

namespace DewIt.Model.Processing.Processes;

public class ProcessValidator : ProcessEvents, IProcessValidator
{
    public ProcessValidator(ILogger logger) : base(logger)
    {
    }

    public IResult ValidateStepsBeforeExecution(IEnumerable<IProcessStep> steps)
    {
        var objective = ProcessingStrings.msg_Validating_Steps;

        // ensure all step UUIDs are unique
        var duplicates = steps.GroupBy(it => it.UUID)
            .Where(g => g.Count() > 1)
            .Select(it => it.Key.ToString())
            .ToList();


        if (duplicates.Any())
        {
            var dupString = string.Join(", ", duplicates);
            var reason = string.Format(ProcessingStrings.ERROR_Steps_not_unique, dupString);
            var ex = new InvalidOperationException(reason);
            return ResultFactory.FAILURE(objective, reason, ex);
        }

        return ResultFactory.SUCCESS(objective);
    }

    public IResult ValidateAndCleanupStep(int stepNumber, int count, IProcessStep step)
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
            var result = ResultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepValidationFailed(step, result, stepNumber, count);
            return result;
        }
    }
}
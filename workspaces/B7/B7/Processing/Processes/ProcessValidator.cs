using System;
using System.Globalization;
using B7.Processing.Results;
using Microsoft.Extensions.Logging;

namespace B7.Processing.Processes;

public class ProcessValidator : ProcessEvents, IProcessValidator
{
    public ProcessValidator(ILogger logger) : base(logger)
    {
    }

    public IResult ValidateAndCleanupStep(int stepNumber, int totalSteps, IProcessStep step)
    {
        OnStepValidationStarting(step, stepNumber, totalSteps);

        try
        {
            var validation = step.ValidateAndCleanup();
            if (validation.IsSkipped())
            {
                OnStepValidationSkipped(step, (validation as Skipped)!, stepNumber, totalSteps);
            }
            else if (validation.IsSuccess())
            {
                OnStepValidationComplete(step, (validation as Success)!, stepNumber, totalSteps);
            }
            else
            {
                OnStepValidationFailed(step, (validation as Failure)!, stepNumber, totalSteps);
            }

            return validation;
        }
        catch (Exception ex)
        {
            var result = ResultFactory.FAILURE(step.Title, ex.Message, ex);
            OnStepValidationFailed(step, result, stepNumber, totalSteps);
            return result;
        }
    }

    public IResult ValidateStepsBeforeExecution(IEnumerable<IProcessStep> steps)
    {
        var objective = String.Format(ProcessingStrings.msg_ValidatingSteps, CultureInfo.CurrentCulture);

        // ensure all step UUIDs are unique
        List<string> duplicates = steps.GroupBy(it => it.UUID)
            .Where(g => g.Count() > 1)
            .Select(it => it.Key.ToString())
            .ToList();

        if (duplicates.Any())
        {
            var dupString = string.Join(", ", duplicates);
            var reason = string.Format(ProcessingStrings.ERROR_StepsNotUnique, dupString);
            var ex = new InvalidOperationException(reason);
            return ResultFactory.FAILURE(objective, reason, ex);
        }

        return ResultFactory.SUCCESS(objective);
    }
}


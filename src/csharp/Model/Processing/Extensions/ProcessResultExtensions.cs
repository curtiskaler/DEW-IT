using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public static class ProcessResultExtensions
{
    public static List<Failure> GetFailures(this IProcessResult multiResult)
    {
        if (multiResult == null)
            throw new ArgumentNullException(nameof(multiResult));

        return multiResult.StepsAndResults
            .Select(it => it.Result)
            .GetFailures();
    }

    public static List<string> GetFailureReasons(this IProcessResult multiResult)
    {
        if (multiResult == null)
            throw new ArgumentNullException(nameof(multiResult));

        var failures = multiResult.GetFailures();
        return failures
            .Select(it =>
                string.Format(ProcessingStrings.msg_Step_Failed, it.Objective) + Environment.NewLine + it.Reason)
            .ToList();
    }

    public static string GetFirstFailureReason(this IProcessResult multiResult)
    {
        if (multiResult == null)
            throw new ArgumentNullException(nameof(multiResult));

        var reasons = multiResult.GetFailureReasons();

        if (reasons.Count == 0) return "";

        return reasons.Count == 1 ? reasons[0] : "Process failed for multiple reasons.";
    }
}
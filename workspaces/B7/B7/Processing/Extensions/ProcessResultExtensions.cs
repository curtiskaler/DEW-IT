using System.Globalization;

namespace B7.Processing;

public static class ProcessResultExtensions
{
    public static List<Failure> GetFailures(this IProcessResult multiResult)
    {
        if (multiResult == null) { throw new ArgumentNullException(nameof(multiResult)); }

        return multiResult.StepsAndResults
            .Select(it => it.Result)
            .GetFailures();
    }

    public static List<string> GetFailureReasons(this IProcessResult multiResult)
    {
        if (multiResult == null) { throw new ArgumentNullException(nameof(multiResult)); }

        var failures = multiResult.GetFailures();
        return failures.Select(it =>
            string.Format(ProcessingStrings.msg_StepFailed, it.Objective, CultureInfo.CurrentCulture) + Environment.NewLine + it.Reason
        ).ToList();
    }

    public static string GetFirstFailureReason(this IProcessResult multiResult)
    {
        if (multiResult == null) { throw new ArgumentNullException(nameof(multiResult)); }
        var reasons = multiResult.GetFailureReasons();
        if (reasons.Count == 0) return "";
        return (reasons.Count == 1) ? reasons[0] : string.Format(ProcessingStrings.msg_ProcessFailedForMultipleReasons, CultureInfo.CurrentCulture);
    }
}


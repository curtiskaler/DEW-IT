using System.Runtime.InteropServices;

namespace DewIt.Model.Processing.Results;

public sealed class ResultFactory : IResultFactory
{
    public Success SUCCESS([Optional] string reason)
    {
        return new Success(reason);
    }

    public Failure FAILURE(string reason, Exception exception, FailureAction failureAction)
    {
        return new Failure(reason, exception, failureAction);
    }

    public Skipped SKIP(string reason)
    {
        return new Skipped(reason);
    }
}
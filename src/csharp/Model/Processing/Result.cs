using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public static class Result
{
    public static Success SUCCESS()
    {
        return SUCCESS("");
    }

    public static Success SUCCESS(string message)
    {
        return new Success(message);
    }

    public static Success SUCCESS<TOut>(TOut output)
    {
        return SUCCESS(output, "");
    }

    public static Success SUCCESS<TOut>(TOut output, string message)
    {
        return new Success<TOut>(output, message);
    }

    public static Failure FAILURE(string reason, Exception ex)
    {
        return FAILURE(reason, ex.ToList(), FailureAction.STOP);
    }
    
    public static Failure FAILURE(string reason, List<Exception> exceptions)
    {
        return FAILURE(reason, exceptions, FailureAction.STOP);
    }

    public static Failure FAILURE(string reason, Exception ex, FailureAction severity)
    {
        return FAILURE(reason, ex.ToList(), severity);
    }

    public static Failure FAILURE(string reason, List<Exception> exceptions, FailureAction severity)
    {
        return new Failure(reason, exceptions, severity);
    }

    public static Skipped SKIP(string reason)
    {
        return new Skipped(reason);
    }
}
using System.Runtime.InteropServices;

namespace DewIt.Model.Processing.Results;

public sealed class ResultFactory : IResultFactory
{
    public Success SUCCESS(string objective, [Optional] string message)
    {
        return new Success(objective, message);
    }

    public Success<T> SUCCESS<T>(string objective, T result, [Optional] string message)
    {
        return new Success<T>(objective, result, message);
    }

    public Failure FAILURE(string objective, string reason, Exception exception)
    {
        return new Failure(objective, reason, exception);
    }
    public Failure FAILURE(string objective, string reason, IEnumerable<Exception> exceptions)
    {
        return new Failure(objective, reason, exceptions);
    }

    public Failure<T> FAILURE<T>(string objective, string reason, IEnumerable<Exception> exceptions)
    {
        return new Failure<T>(objective, reason, exceptions);
    }

    public Failure<T> FAILURE<T>(string objective, string reason, Exception exception)
    {
        return new Failure<T>(objective, reason, exception);
    }

    public Skipped SKIP(string objective, string reason)
    {
        return new Skipped(objective, reason);
    }

    public Skipped<T> SKIP<T>(string objective, string reason)
{
        return new Skipped<T>(objective, reason);
    }
}
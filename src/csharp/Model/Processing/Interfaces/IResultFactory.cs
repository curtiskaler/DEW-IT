using System.Runtime.InteropServices;
using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public interface IResultFactory
{
    Success SUCCESS(string objective, [Optional] string message);
    Success<T> SUCCESS<T>(string objective, T result, [Optional] string message);
    Failure FAILURE(string objective, string reason, Exception exception);
    Failure FAILURE(string objective, string reason, IEnumerable<Exception> exceptions);
    Failure<T> FAILURE<T>(string objective, string reason, Exception exception);
    Failure<T> FAILURE<T>(string objective, string reason, IEnumerable<Exception> exceptions);
    Skipped SKIP(string objective, string reason);
    Skipped<T> SKIP<T>(string objective, string reason);
}
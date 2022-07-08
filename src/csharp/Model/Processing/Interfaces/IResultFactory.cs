using System.Runtime.InteropServices;
using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public interface IResultFactory
{
    Success SUCCESS([Optional] string reason);
    Failure FAILURE(string reason, Exception exception, FailureAction failureAction);
    Skipped SKIP(string reason);
}
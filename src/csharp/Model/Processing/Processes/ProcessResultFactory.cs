using System.Runtime.InteropServices;

namespace DewIt.Model.Processing.Processes;

public static class ProcessResultFactory
{
    public static ProcessResult Create()
    {
        return new ProcessResult();
    }

    public static ProcessResult<TOut> Create<TOut>([Optional] TOut? initialObject) where TOut : new()
    {
        var result = new ProcessResult<TOut>();
        if (initialObject != null)
        {
            result.Object = initialObject;
        }

        return result;
    }
}
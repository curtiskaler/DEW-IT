using System.Runtime.InteropServices;

namespace B7.Processing.Results;

#nullable enable

public static class ProcessResultFactory
{
    public static ProcessResult Create() { return new ProcessResult(); }
    public static ProcessResult<TOut> Create<TOut>([Optional] TOut? initialObject) where TOut : new()
    {
        return new ProcessResult<TOut>(initialObject);
    }
}


using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing;

public static class StepAndResultExtensions
{
    /// <summary> Adds an object to the end of the List. </summary>
    public static void Add(this List<StepAndResult> list, ProcessStep step, IResult result)
    {
        list.Add(new StepAndResult(step, result));
    }

    /// <summary> Adds an object to the end of the List. </summary>
    public static void Add<TOut>(this List<StepAndResult> list, ProcessStep<TOut> step, IResult<TOut> result)
    {
        list.Add(new StepAndResult(step, result));
    }
}
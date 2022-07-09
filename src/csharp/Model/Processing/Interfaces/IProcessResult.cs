namespace DewIt.Model.Processing
{
    public interface IProcessResult
    {
        ResultCode Code { get; }
        bool Failed { get; }
        bool Skipped { get; }
        IStepAndResultCollection StepsAndResults { get; }
        bool Succeeded { get; }
    }

    public interface IProcessResult<out T> : IProcessResult
    {
        T Object { get; }
    }
}
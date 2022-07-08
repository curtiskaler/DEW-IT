using DewIt.Model.Processing.Results;

namespace DewIt.Model.Processing
{
    public interface IProcessResult
    {
        ResultCode Code { get; }
        bool Failed { get; }
        bool Skipped { get; }
        List<StepAndResult> StepsAndResults { get; }
        bool Succeeded { get; }
    }
}
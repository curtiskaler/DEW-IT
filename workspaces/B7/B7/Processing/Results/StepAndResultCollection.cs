namespace B7.Processing.Results;

public class StepAndResultCollection : List<StepAndResult>, IStepAndResultCollection
{
    public StepAndResult Get(IProcessStep step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        return Get(step.UUID);
    }

    public StepAndResult Get(Guid stepUUID)
    {
        var result = Find(it => it.Step.UUID == stepUUID);
        if (result == null)
        {
            throw new ArgumentOutOfRangeException(string.Format(ProcessingStrings.ERROR_StepNotFound, stepUUID));
        }
        return result;
    }

    public void Set(IProcessStep step, IResult result)
    {
        this[step.UUID] = new StepAndResult(step, result);
    }

    public void Add(IProcessStep step, IResult result)
    {
        this.Add(new StepAndResult(step, result));
    }

    public StepAndResult this[IProcessStep step]
    {
        get => Get(step);
        set => this[step.UUID] = value;
    }

    public StepAndResult this[Guid stepUUID]
    {
        get => Get(stepUUID);
        set
        {
            var match = Find(it => it.Step.UUID == stepUUID);
            if (match == null)
            {
                Add(value);
            }
            else
            {
                var index = IndexOf(match);
                this[index] = value;
            }
        }
    }

    public StepAndResultCollection() : this(new List<StepAndResult>()) { }

    public StepAndResultCollection(IEnumerable<StepAndResult> range)
    {
        AddRange(range);
    }
}

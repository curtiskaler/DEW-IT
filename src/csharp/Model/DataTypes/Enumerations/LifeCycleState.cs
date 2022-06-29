namespace DewIt.Model.DataTypes;

/// <summary> The state of an object. </summary>
public abstract class LifeCycleState : Enumeration<LifeCycleState>
{
    public static readonly LifeCycleState NOT_SPECIFIED = new Instance(0, nameof(NOT_SPECIFIED));
    public static readonly LifeCycleState UNINITIALIZED = new Instance(1, nameof(UNINITIALIZED));
    public static readonly LifeCycleState CREATED = new Instance(2, nameof(CREATED));
    public static readonly LifeCycleState DESTROYED = new Instance(3, nameof(DESTROYED));

    protected LifeCycleState(int id, string name) : base(id, name)
    {
    }

    private class Instance : LifeCycleState
    {
        internal Instance(int id, string name) : base(id, name)
        {
        }
    }
}
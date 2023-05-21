using B7.Types;

namespace B7.Lifecycle;

/// <summary> The state of an object. </summary>
public abstract class LifecycleState : Enumeration<LifecycleState>
{
    public static LifecycleState NOT_SPECIFIED => new Instance(0, nameof(NOT_SPECIFIED));
    public static LifecycleState UNINITIALIZED => new Instance(1, nameof(UNINITIALIZED));
    public static LifecycleState CREATED => new Instance(2, nameof(CREATED));
    public static LifecycleState DESTROYED => new Instance(3, nameof(DESTROYED));

    protected LifecycleState(int id, string name) : base(id, name) { }

    private class Instance : LifecycleState
    {
        internal Instance(int id, string name) : base(id, name) { }
    }
}


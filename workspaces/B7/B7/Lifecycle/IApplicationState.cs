namespace B7.Lifecycle;

/// <summary> A generalized object to maintain application state. </summary>
/// <typeparam name="TState"> The type of object. </typeparam>
public interface IApplicationState<TState> : IDisposable where TState : IApplicationState<TState>, new()
{
    /// <summary> The current lifecycle state of the object. </summary>
    LifecycleState State { get; }
}

/// <summary> A generalized object to maintain application state. </summary>
/// <typeparam name="TState"> The type of object which will hold the application state. </typeparam>
public abstract class ApplicationState<TState> : IApplicationState<TState> where TState : IApplicationState<TState>, new()
{
    protected ApplicationState()
    {
        State = LifecycleState.UNINITIALIZED;
    }

    /// <summary> The current lifecycle state of the application state object. </summary>
    public LifecycleState State { get; protected set; }

    #region IDisposable

    // While this abstraction does not own any unmanaged resources or
    // disposable objects, it is likely to have subtypes that do.
    // Here we include an empty implementation, as in System.IO.Stream.

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    #endregion
}

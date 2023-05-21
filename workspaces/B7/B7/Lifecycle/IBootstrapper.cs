namespace B7.Lifecycle;

public interface IBootstrapper
{
    /// <summary> Load the data required to start the application, and execute any startup algorithms. </summary>
    void Bootstrap();
}

/// <summary> An object that initializes the application and application date. </summary>
public interface IBootstrapper<TState> where TState : IApplicationState<TState>, new()
{
    /// <summary> Load the data required to start the application, and execute any startup algorithms. </summary>
    TState Bootstrap();
}
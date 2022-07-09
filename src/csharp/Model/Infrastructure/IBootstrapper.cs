namespace DewIt.Model.Infrastructure
{
    public interface IBootstrapper
    {
        /// <summary> Load the data required to start the application. </summary>
        void Bootstrap();
    }

    public interface IBootstrapper<out TState> where TState : IApplicationState<TState>
    {
        /// <summary> Load the data required to start the application. </summary>
        TState Bootstrap();
    }
}
namespace DewIt.Model.Infrastructure
{
    public interface IBootstrapper<in TState> where TState : IApplicationState<TState>
    {
        /// <summary> Load the data required to start the application. </summary>
        void Bootstrap(TState state);
    }
}
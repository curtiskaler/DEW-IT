using DewIt.Model.DataTypes;

namespace DewIt.Model.Infrastructure;

public interface IApplicationState<out TState> where TState : IApplicationState<TState>
{
    TState Initialize(IBootstrapper<TState> bootstrapper);
}
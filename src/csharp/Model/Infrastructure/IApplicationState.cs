namespace DewIt.Model.Infrastructure;

public interface IApplicationState<TState> where TState: IApplicationState<TState>
{
    TState Initialize();
}
namespace DewIt.Model.Infrastructure;

public interface IApplicationState<out TState> where TState: IApplicationState<TState>
{
}
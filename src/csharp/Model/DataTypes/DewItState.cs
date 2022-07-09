using DewIt.Model.Infrastructure;

namespace DewIt.Model.DataTypes
{
    public interface IDewItState : IApplicationState<DewItState>
    {
    }

    public class DewItState : IDewItState
    {
        public DewItState()
        {
        }
    }
}
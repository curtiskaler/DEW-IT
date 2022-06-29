using DewIt.Model.Infrastructure;

namespace DewIt.Model.DataTypes
{
    public class DewItState : IApplicationState<DewItState>
    {
        public LifeCycleState Status { get; set; } = LifeCycleState.UNINITIALIZED;

        public static readonly DewItState UNINITIALIZED = new() { Status = LifeCycleState.UNINITIALIZED };

        public DewItState Initialize(IBootstrapper<DewItState> bootstrapper)
        {
            System.Diagnostics.Debug.WriteLine("Initializing!");
            var state = DewItState.UNINITIALIZED;
            bootstrapper.Bootstrap(state);
            return state;
        }
    }
}
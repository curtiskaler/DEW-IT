using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;

namespace DewIt.Client.Dev;

internal class DevBootstrapper : IBootstrapper<DewItState>
{
    public void Bootstrap(DewItState state)
    {
        System.Diagnostics.Debug.WriteLine("Dev bootstrapping!");
    }
}
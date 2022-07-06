using DewIt.Model.DataTypes;
using DewIt.Model.Infrastructure;
using DewIt.Model.Persistence;

namespace DewIt.Client.Dev;

internal class DevBootstrapper : IBootstrapper<DewItState>
{
    private readonly IRepositoryCollection _repositories;

    public DevBootstrapper(IRepositoryCollection repositoryCollection)
    {
        _repositories = repositoryCollection ?? throw new ArgumentNullException(nameof(repositoryCollection));
    }


    public void Bootstrap(DewItState state)
    {
        System.Diagnostics.Debug.WriteLine("Dev bootstrapping!");
        _repositories.Initialize();
    }
}
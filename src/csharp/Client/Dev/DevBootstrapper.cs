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
        
        // initialize the repositories (make sure the repos exist and can be accessed
        try
        {
            _repositories.Initialize();
        }
        catch (Exception ex)
        {

        }


    }
}
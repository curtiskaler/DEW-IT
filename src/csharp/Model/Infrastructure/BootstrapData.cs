using DewIt.Model.Persistence;

namespace DewIt.Model.Infrastructure;

public interface IBootstrapData
{
    IRepositoryCollection Repositories { get; }
}

public class BootstrapData : IBootstrapData
{
    public IRepositoryCollection Repositories { get; }

    public BootstrapData(IRepositoryCollection repositories)
    {
        Repositories = repositories;
    }
}
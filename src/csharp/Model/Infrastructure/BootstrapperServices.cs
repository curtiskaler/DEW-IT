using DewIt.Model.Persistence;

namespace DewIt.Model.Infrastructure;

public interface IBootstrapperServices
{
    IRepositoryCollection Repositories { get; }
}

public class BootstrapperServices : IBootstrapperServices
{
    public IRepositoryCollection Repositories { get; }

    public BootstrapperServices(IRepositoryCollection repositories)
    {
        Repositories = repositories;
    }
}
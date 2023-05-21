
using DewIt.Client.Persistence;

namespace DewIt.Client.Bootstrapping;

public interface IBootstrapperServices
{
    IRepositoryCollection Repositories { get; }
}

public class BootstrapperServices : IBootstrapperServices
{
    public IRepositoryCollection Repositories { get; }

    public BootstrapperServices(IRepositoryCollection repositories)
    {
        Repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
    }
}


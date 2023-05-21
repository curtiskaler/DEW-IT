using System.Collections.Concurrent;
using System.Numerics;
using B7.Persistence;
using B7.Types;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Persistence;

public static class DewItRepos
{
    public const string BOARDS = "Boards";
    public const string CARDS = "Cards";
    public const string GROUPS = "Groups";
    public const string LANES = "Lanes";
    public const string USERS = "Users";
}

public interface IRepositoryCollection : IInitializable
{
    IRepository<Board> Boards { get; }
    IRepository<Lane> Lanes { get; }
    IRepository<Card> Cards { get; }
    IRepository<Group> Groups { get; }
    IRepository<User> Users { get; }
}

public class RepositoryCollection : IRepositoryCollection, IInitializable
{
    private ILogger _logger;

    public IRepository<Board> Boards => _repositories[DewItRepos.BOARDS] as IRepository<Board>;
    public IRepository<Card> Cards => _repositories[DewItRepos.CARDS] as IRepository<Card>;
    public IRepository<Group> Groups => _repositories[DewItRepos.GROUPS] as IRepository<Group>;
    public IRepository<Lane> Lanes => _repositories[DewItRepos.LANES] as IRepository<Lane>;
    public IRepository<User> Users => _repositories[DewItRepos.USERS] as IRepository<User>;

    ConcurrentDictionary<string, IRepository> _repositories { get; }

    public RepositoryCollection(ILogger logger,
        IRepository<Lane> lanes, IRepository<Card> cards,
        IRepository<Group> groups, IRepository<User> users
        )
    {

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repositories = new ConcurrentDictionary<string, IRepository>();
        TryAdd(lanes);
        TryAdd(cards);
        TryAdd(groups);
        TryAdd(users);
    }

    public void Initialize()
    {
        _logger.Log(LogLevel.Information, "Initializing Repositories:");

        // order matters here
        this.Cards.Initialize();
        this.Groups.Initialize();
        this.Lanes.Initialize();
        this.Users.Initialize();

        _logger.Log(LogLevel.Information, "...Repositories initialized!");
    }

    private void TryAdd(IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        bool added = _repositories.TryAdd(repository.RepositoryKey, repository);
        if (!added)
        {
            throw new ArgumentException(null, nameof(repository));
        }
    }
}

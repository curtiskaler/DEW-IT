using B7.Identifiers;
using Microsoft.Extensions.Logging;

namespace B7.Persistence;

public interface IInitializable
{
    void Initialize();
}
public interface IRepository : IInitializable
{
    Type EntityType { get; }
    String RepositoryKey { get; }
}
public interface IRepository<T> : IRepository where T : IUniqueObject, new()
{
    Task CascadeDelete(T item);
    Task DeleteItem(Guid uuid);
    Task DeleteMany(List<T> items, bool cascade = true);

    Task<List<T>> GetAll();
    Task<T> GetItem(Guid uuid);

    Task<List<T>> BulkUpdate(List<T> items);
    Task<T> UpdateItem(T item);

    Task<List<T>> BulkSave(List<T> items);
    Task<T> SaveItem(T item);
}

public abstract class Repository : IRepository
{
    protected readonly ILogger _logger;

    public Type EntityType { get; }
    public string RepositoryKey { get; }

    public abstract void Initialize();

    protected Repository(ILogger logger, Type type, string key)
    {
        _logger = logger;
        EntityType = type;
        RepositoryKey = key;
    }
}
public abstract class Repository<T> : Repository, IRepository<T> where T : IUniqueObject, new()
{

    protected Repository(ILogger logger, Type type, string key) : base(logger, type, key)
    {
    }

    public abstract Task<List<T>> BulkSave(List<T> items);

    public abstract Task<List<T>> BulkUpdate(List<T> items);

    public abstract Task CascadeDelete(T item);

    public abstract Task DeleteMany(List<T> items, bool cascade = true);

    public abstract Task DeleteItem(Guid uuid);

    public abstract Task<List<T>> GetAll();

    public abstract Task<T> GetItem(Guid uuid);

    public abstract Task<T> SaveItem(T item);

    public abstract Task<T> UpdateItem(T item);
}
using Microsoft.Extensions.Logging;
using SQLite;

namespace DewIt.Model.Persistence;

public interface IRepository<T> where T : new()
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

    void Initialize();
}

public abstract class Repository<T> : IRepository<T> where T : new()
{
    private readonly Lazy<SQLiteConnection> _databaseConnectionHolder;
    protected readonly ILogger _logger;

    protected SQLiteConnection Database
    {
        get
        {
            var database = _databaseConnectionHolder.Value;
            database.CreateTable<T>();
            return database;
        }
    }

    protected Repository(ILogger logger, IResource resource)
    {
        _logger = logger;
        var dbPath = resource.GetPath();
        _databaseConnectionHolder = new Lazy<SQLiteConnection>(
            () => new SQLiteConnection(dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
    }


    public virtual Task DeleteItem(Guid uuid)
    {
        Database.Delete<T>(uuid);
        return Task.CompletedTask;
    }

    public virtual Task<List<T>> GetAll()
    {
        return Task.FromResult(Database.Table<T>().ToList());
    }

    public virtual Task<T> GetItem(Guid uuid)
    {
        return Task.FromResult(Database.Get<T>(uuid));
    }

    public virtual Task<T> UpdateItem(T item)
    {
        Database.Update(item);
        return Task.FromResult(item);
    }

    public virtual Task<T> SaveItem(T item)
    {
        Database.Insert(item);
        return Task.FromResult(item);
    }


    public virtual Task CascadeDelete(T item)
    {
        throw new NotImplementedException();
    }

    public virtual Task DeleteMany(List<T> items, bool cascade = true)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<T>> BulkUpdate(List<T> items)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<T>> BulkSave(List<T> items)
    {
        throw new NotImplementedException();
    }

    public void Initialize()
    {
        Database.CreateTable<T>();
    }
}
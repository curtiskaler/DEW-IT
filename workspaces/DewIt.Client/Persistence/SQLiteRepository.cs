using B7.Identifiers;
using B7.Persistence;
using Microsoft.Extensions.Logging;
using SQLite;

namespace DewIt.Client.Persistence;

public abstract class SQLiteRepository<T> : Repository<T> where T : IUniqueObject, new()
{
    private readonly Lazy<SQLiteConnection> _databaseConnectionHolder;

    protected SQLiteConnection Database
    {
        get
        {
            var database = _databaseConnectionHolder.Value;
            database.CreateTable<T>();
            return database;
        }
    }

    protected SQLiteRepository(ILogger logger, IResource resource, string key) : base(logger, typeof(T), key)
    {
        var dbPath = resource.GetPersistencePath();
        _databaseConnectionHolder = new Lazy<SQLiteConnection>(
            () => new SQLiteConnection(dbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));

    }

    public override Task<List<T>> BulkSave(List<T> items)
    {
        throw new NotImplementedException();
    }

    public override Task<List<T>> BulkUpdate(List<T> items)
    {
        throw new NotImplementedException();
    }

    public override Task CascadeDelete(T item)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteItem(Guid uuid)
    {
        Database.Delete<T>(uuid);
        return Task.CompletedTask;
    }

    public override Task DeleteMany(List<T> items, bool cascade = true)
    {
        throw new NotImplementedException();
    }

    public override Task<List<T>> GetAll()
    {
        return Task.FromResult(Database.Table<T>().ToList());
    }

    public override Task<T> GetItem(Guid uuid)
    {
        return Task.FromResult(Database.Get<T>(uuid));
    }

    public override void Initialize()
    {
        Database.CreateTable<T>();
    }

    public override Task<T> SaveItem(T item)
    {
        Database.Insert(item);
        return Task.FromResult(item);
    }

    public override Task<T> UpdateItem(T item)
    {
        Database.Update(item);
        return Task.FromResult(item);
    }
}
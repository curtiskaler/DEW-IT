namespace DewIt.Model.DataTypes
{
    public interface IHistoryEntry : IHaveTimestamp, IHaveOwner
    {
        HistoryEntryType ChangeType { get; set; }
        IUser MadeBy { get; set; }
    }

    public interface IHistoryEntry<T> : IHistoryEntry
    {
        T Target { get; set; }
    }

    public interface IAddRemoveChildEntry<T, S> : IHistoryEntry<S>
    {
        T Item { get; set; }
    }

    public interface IUpdateEntry<T> : IHistoryEntry
    {
        T Previous { get; set; }
        T Current { get; set; }
    }

    public interface IPropertyChangeEntry<TEntity, TProp> : IHistoryEntry<TEntity>, IUpdateEntry<TProp>
    {
        string Property { get; set; }
    }

    public interface IMoveEntry : IUpdateEntry<IGenealogy>
    {
    }
}